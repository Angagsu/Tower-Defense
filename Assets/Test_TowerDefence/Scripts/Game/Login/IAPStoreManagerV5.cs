using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.Services.CloudCode;
using Unity.Services.CloudCode.GeneratedBindings;
using Unity.Services.CloudCode.GeneratedBindings.HayguardRiseOfHighlandCloud;
using Unity.Services.Economy;
using Unity.Services.Economy.Model;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Security;

public class IAPStoreManagerV5 : MonoBehaviour
{
    public event Action<string> SuccessfullyPurchased;
    public event Action<string> PurchaseFailed;

    [Header("Dependencies")]
    [SerializeField] private PlayerEconomyManager playerEconomyManager;

    private const string GOLD_PURCHASE_100_ID = "GOLD_PURCHASE_100";
    private const string GOOGLE_PUBLICK_KEY = "";

    private StoreController storeController;
    private StoreServiceBindings bindings;
    private CrossPlatformValidator googleValidator;

    private bool isPurchaseInProgress;



    private void Start()
    {
        playerEconomyManager.EconomyConfigSynced += InitializeIAPAsync;

        bindings = new StoreServiceBindings(CloudCodeService.Instance); 
    }
    private void OnDestroy()
    {
        playerEconomyManager.EconomyConfigSynced -= InitializeIAPAsync;
        UnsubscibeIAPEvents();
    }

    public void PurchaseGold()
    {
        if (isPurchaseInProgress)
        {
            Debug.LogWarning("[IAP] Purchase already in progress.");
            return;
        }

        isPurchaseInProgress = true;
        storeController.PurchaseProduct(GOLD_PURCHASE_100_ID);
    }

    private async void InitializeIAPAsync()
    {
        storeController = UnityIAPServices.StoreController();

        SubscribeIAPEvents();

        try
        {
            await storeController.Connect();
            Debug.Log("[IAP] Connected to store");

            var productDefs = BuildProductDefinitionsFromEconomy();

            if(productDefs.Count == 0)
            {
                Debug.LogWarning("[IAP] No real mony products found in Economy config.");
                return;
            }

            storeController.FetchProducts(productDefs);
        }
        catch (Exception e)
        {
            Debug.LogError($"[IAP] Connect failed: {e.Message}");
        }

        InitializeReceiptValidatorsIfNeeded();
    }

    private void InitializeReceiptValidatorsIfNeeded()
    {
#if !UNITY_EDITOR

if (Application.platform == RuntimePlatform.Android)
        {
            try
            {
                googleValidator = new CrossPlatformValidator(GooglePlayTangle.Data(), Application.identifier);
                Debug.Log("[IAP] Google receipt validator initialized.");
            }
            catch (Exception e)
            {
                Debug.LogWarning($"[IAP] Validator init skipped: {e.Message}");
            }
        }
 
#endif
    }

    private List<ProductDefinition> BuildProductDefinitionsFromEconomy()
    {
        var productDefinitions = new List<ProductDefinition>();
        var realMoney = EconomyService.Instance.Configuration.GetRealMoneyPurchases();
        LogRealPurchasesFromConfig(realMoney);

        foreach (var purchase in realMoney)
        {
            var def = new ProductDefinition(id: purchase.Id, type: ProductType.Consumable);
            productDefinitions.Add(def);
        }

        Debug.Log($"[IAP] Prepard {productDefinitions.Count} ProductDefinitions for featch.");
        return productDefinitions;
    }

    private void LogRealPurchasesFromConfig(List<RealMoneyPurchaseDefinition> realMoneyPurchases)
    {
        Debug.Log($"Real purchases from economy config:\n{JsonConvert.SerializeObject(realMoneyPurchases, Formatting.Indented)}");
    }

    private void SubscribeIAPEvents()
    {
        if(storeController == null) return;

        storeController.OnProductsFetched += OnProductsFetched;
        storeController.OnProductsFetchFailed += OnProductsFetchFailed;

        storeController.OnPurchasesFetched += OnPurchasesFetched;
        storeController.OnPurchasesFetchFailed += OnPurchasesFetchFailed;

        storeController.OnPurchasePending += OnPurchasePending;
        storeController.OnPurchaseConfirmed += OnPurchaseConfirmed;
        storeController.OnPurchaseFailed += OnPurchaseFailed;

        storeController.OnStoreDisconnected += OnStoreDisconnected;
    }

    private void OnStoreDisconnected(StoreConnectionFailureDescription desc)
    {
        Debug.LogError($"[IAP] Store disconected: {desc.Message}");
    }



    private async void OnPurchasePending(PendingOrder pending)
    {
        try
        {
            Debug.Log($"Full receipt JSON: {pending.Info.Receipt}");

            var firstItem = pending.CartOrdered.Items().FirstOrDefault();
            var pid = firstItem?.Product?.definition?.id;

            if (string.IsNullOrEmpty(pid))
            {
                Debug.LogError("[IAP] Pending order has no product id.");
                PurchaseFailed?.Invoke("No product id in pending order");
                return;
            }

            var product = storeController?.GetProductById(pid);

            if(product == null)
            {
                Debug.LogError($"[IAP] Product not found in controller: {pid}.");
                PurchaseFailed?.Invoke($"Product not found: {pid}");
                return;
            }

            Debug.Log($"[IAP] Pending purchase: {product.definition.id}");

            var receipt = pending.Info.Receipt;

            if (!ValidateIfGoogle(receipt))
            {
                Debug.LogError("[IAP] Google receipt validation failed.");
                PurchaseFailed?.Invoke("Invalid receipt for " + product.definition.id);
                return;
            }

            PlayerEconomyData updated = await bindings.ProcessRealMoneyPurchase(product.definition.id, receipt,
                (double)product.metadata.localizedPrice, product.metadata.isoCurrencyCode);

            if (updated == null)
            {
                Debug.LogError("[IAP] Cloud Code returned null economy data.");
                PurchaseFailed?.Invoke("Server processing faild for " + product.definition.id);
                return;
            }

            playerEconomyManager.HandleEconomyUpdate(updated);
            storeController.ConfirmPurchase(pending);

            Debug.Log($"[IAP] Confirmed purchase: {product.definition.id}");
        }
        catch (Exception e)
        {
            Debug.LogError($"[IAP] Error processing pending order: {e.Message}");
            PurchaseFailed?.Invoke("Purchase failed " + e.Message);
        }
    }

    private void OnPurchaseConfirmed(Order order)
    {
        isPurchaseInProgress = false;

        if(order is FailedOrder failedOrder)
        {
            Debug.LogWarning($"[IAP] Confirmation failed: {failedOrder.FailureReason}");
            return;
        }

        var purchasedProduct = order.CartOrdered.Items().FirstOrDefault()?.Product;

        Debug.Log($"[IAP] Purchase confirmed: {purchasedProduct?.definition.id} | Tx: {order.Info?.TransactionID}");
        SuccessfullyPurchased?.Invoke($"Purchase confirmed: {purchasedProduct?.definition.id}");
    }
    private void OnPurchaseFailed(FailedOrder failed)
    {
        isPurchaseInProgress = false;

        Debug.LogError($"[IAP] Purchase faild: {failed.FailureReason.ToString()}");
        PurchaseFailed?.Invoke($"Purchase failed: {failed.FailureReason.ToString()}");
    }

    private bool ValidateIfGoogle(string receipt)
    {
        if (googleValidator == null) return true;

        try
        {
            var result = googleValidator.Validate(receipt);
            foreach (var r in result)
            {
                Debug.Log($"[IAP] Receipt OK: {r.productID} @ {r.purchaseDate} | Tx: {r.transactionID}");
            }

            return true;
        }
        catch (IAPSecurityException e)
        {
            Debug.LogError($"[IAP] Receipt invalid: {e.Message}");
            return false;
        }
    }

    private void OnPurchasesFetched(Orders orders)
    {
        // Process purchases , e.g. check for entitlements from completed orders
    }

    private void OnPurchasesFetchFailed(PurchasesFetchFailureDescription failure)
    {
        Debug.LogError($"[IAP] Purchases fetch failed: {failure.FailureReason}");
    }

    private void OnProductsFetched(List<Product> products)
    {
        storeController.FetchPurchases();

        LogProductsFetched(products);
    }

    

    private void OnProductsFetchFailed(ProductFetchFailed failure)
    {
        Debug.LogError($"[IAP] Product fetch failed: {failure.FailureReason}");
    }

    private void LogProductsFetched(List<Product> products)
    {
        Debug.Log($"[IAP] Products fetched: {products.Count}");

        foreach (var product in products)
        {
            Debug.Log($"[IAP] {product.definition.id} | {product.metadata.localizedTitle} | {product.metadata.localizedPriceString}");
        }
    }

    private void UnsubscibeIAPEvents()
    {
        if (storeController == null) return;

        storeController.OnProductsFetched -= OnProductsFetched;
        storeController.OnProductsFetchFailed -= OnProductsFetchFailed;

        storeController.OnPurchasesFetched -= OnPurchasesFetched;
        storeController.OnPurchasesFetchFailed -= OnPurchasesFetchFailed;

        storeController.OnPurchasePending -= OnPurchasePending;
        storeController.OnPurchaseConfirmed -= OnPurchaseConfirmed;
        storeController.OnPurchaseFailed -= OnPurchaseFailed;

        storeController.OnStoreDisconnected -= OnStoreDisconnected;
    }
}
