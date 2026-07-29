using Newtonsoft.Json;
using System;
using Unity.Services.CloudCode;
using Unity.Services.CloudCode.GeneratedBindings;
using Unity.Services.Economy;
using UnityEngine;

public class VirtualStoreManager : MonoBehaviour
{
    [SerializeField] private  PlayerEconomyManager playerEconomyManager;

    [Header("Purchase IDs")]
    [SerializeField] private string cardPurchaseId = "CARD_VIRTUAL_PURCHASE";

    private int currentCardCost;
    private const int defaultCardPurchaseCost = 20;

    private StoreServiceBindings bindings;



    private void OnEnable()
    {
        playerEconomyManager.EconomyConfigSynced += InitializeVirtualStore;
    }

    private void OnDisable()
    {
        playerEconomyManager.EconomyConfigSynced -= InitializeVirtualStore;
    }

    private void Start()
    {
        bindings = new StoreServiceBindings(CloudCodeService.Instance);
        currentCardCost = defaultCardPurchaseCost;
    }

    private void InitializeVirtualStore()
    {
        try
        {
            LogVirtualPurchasesFromConfig();
            InitializePurchaseCosts();
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Faild to sync Economy configuration: {ex.Message}");
        }
    }

    private void LogVirtualPurchasesFromConfig()
    {
        var virtualPurchases = EconomyService.Instance.Configuration.GetVirtualPurchases();
        string virtualPurchasesJson = JsonConvert.SerializeObject(virtualPurchases, Formatting.Indented);
        Debug.Log($"Virtual purchases from economy config: {virtualPurchasesJson}");
    }

    private void InitializePurchaseCosts()
    {
        try
        {
            var purchaseDefinition = EconomyService.Instance.Configuration.GetVirtualPurchase(cardPurchaseId);

            if (purchaseDefinition == null)
            {
                Debug.LogWarning($"Virtual purchase {cardPurchaseId} not found. Using default cost.");
                return;
            }

            foreach (var cost in purchaseDefinition.Costs)
            {
                if (cost.Item.GetReferencedConfigurationItem().Id == PlayerEconomyManager.GOLD_CURRENCY_KEY)
                {
                    currentCardCost = cost.Amount;
                    Debug.Log($"Card cost set to {currentCardCost} gold");
                    return;
                }
            }

            Debug.LogWarning($"Could not find gold cost for purchase {cardPurchaseId}. Using default value.");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error initializing purchase costs: {ex.Message}. Using default values.");
        }
    }

    public async void PurchaseCard()
    {
        if (!CanAffordVirtualPurchase(currentCardCost))
        {
            Debug.LogWarning($"Not enough gold! Need {currentCardCost}, have {playerEconomyManager.Gold}");
            return;
        }

        try
        {
            var economyData = await bindings.VirtualPurchaseCard();

            Debug.Log($"Successfully Purchased - Card: {cardPurchaseId}");

            playerEconomyManager.HandleEconomyUpdate(economyData);
        }
        catch (CloudCodeException ex)
        {
            Debug.LogException(ex);
        }
    }

    private bool CanAffordVirtualPurchase(int cost)
    {
        var gold = playerEconomyManager.Gold;

        return gold >= cost;
    }
}
