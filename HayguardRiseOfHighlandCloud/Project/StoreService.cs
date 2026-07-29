using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Services.CloudCode.Apis;
using Unity.Services.CloudCode.Core;
using Unity.Services.CloudCode.Shared;
using Unity.Services.Economy.Model;

namespace HayguardRiseOfHighlandCloud
{
    public class StoreService
    {
        private const string CARD_PURCHASE_ID = "CARD_VIRTUAL_PURCHASE";
        

        private readonly ILogger<StoreService> logger;
        private PlayerEconomyService playerEconomyService;

        public StoreService(ILogger<StoreService> logger, PlayerEconomyService playerEconomyService)
        {
            this.logger = logger;
            this.playerEconomyService = playerEconomyService;
        }


        [CloudCodeFunction("VirtualPurchaseCard")]
        public async Task<PlayerEconomyData> VirtualPurchaseCard(IExecutionContext context, IGameApiClient gameApiClient)
        {
            try
            {
                await ProcessVirtualPurchase(context, gameApiClient, CARD_PURCHASE_ID);
                await playerEconomyService.CleanUpNullOrZeroAmountItems(context, gameApiClient, PlayerEconomyService.CARD_CURRENCY_KEY);
                await playerEconomyService.AddOrUpdateInventoryItemAmount(context, gameApiClient, PlayerEconomyService.CARD_CURRENCY_KEY, 1);

                return await playerEconomyService.GetPlayerEconomyData(context, gameApiClient);
            }
            catch (ApiException ex)
            {
                logger.LogError(ex, $"Failed to purchase Card: {context.PlayerId}");
                throw new Exception($"Failed to purchase Card: {ex.Message}", ex);
            }
        }

        private async Task ProcessVirtualPurchase(IExecutionContext context, IGameApiClient gameApiClient, string virtualPurchaseID)
        {
            try
            {
                var purchaseRequest = new PlayerPurchaseVirtualRequest(virtualPurchaseID);

                var purchaseResponse = await gameApiClient.EconomyPurchases.MakeVirtualPurchaseAsync(
                    context,
                    context.AccessToken,
                    context.ProjectId,
                    context.PlayerId ?? throw new InvalidOperationException("PlayerID is null"),
                    purchaseRequest
                    );

                if(purchaseResponse == null || purchaseResponse.Data == null || purchaseResponse.Data.Rewards == null)
                {
                    logger.LogWarning($"Invalid purchase response structure for {virtualPurchaseID}");
                    return;
                }

                List<InventoryExchangeItem> rewardItem = purchaseResponse.Data.Rewards.Inventory;

                logger.LogInformation($"Virtually purchased {virtualPurchaseID}. Reward: " + JsonConvert.SerializeObject(rewardItem));
            }
            catch (ApiException ex)
            {
                logger.LogError(ex, $"Faild to process card purchase: {context.PlayerId}");
                throw;
            }
        }

        [CloudCodeFunction("ProcessRealMoneyPurchase")]
        public async Task<PlayerEconomyData> ProcessRealMoneyPurchase(
            IExecutionContext context,
            IGameApiClient gameApiClient,
            string productId,
            string receipt,
            double localPrice,
            string currencyCode)
        {
            try
            {
                //await ValidatePlayerEligibility(context, gameApiClient, productId);
                await ProcessStoreReceipt(context, gameApiClient, productId, receipt, localPrice, currencyCode);

                return await playerEconomyService.GetPlayerEconomyData(context, gameApiClient)
                    ?? throw new InvalidOperationException("Failed to get player economy data");

            }
            catch (ApiException ex)
            {
                logger.LogWarning($"Economy API error processing purchase for product {productId}: {ex.Message}");
                throw;
            }
            catch (Exception e)
            {
                logger.LogWarning($"Unexpected error processing purchase for product {productId}: {e.Message}");
                throw;
            }

        }

        private async Task ProcessStoreReceipt(
            IExecutionContext context,
            IGameApiClient gameApiClient,
            string productId,
            string receipt,
            double localCost,
            string localCurrency)
        {
            var receiptData = JsonConvert.DeserializeAnonymousType(receipt, new { Store = "", PayLoad = "" })
                ?? throw new JsonException("Unified receipt is null");

            if(string.IsNullOrWhiteSpace(receiptData.Store) || string.IsNullOrWhiteSpace(receiptData.PayLoad))
            {
                throw new JsonException("Unified receipt missing Store/Payload");
            }

            var store = receiptData.Store.ToLowerInvariant();

            switch (store)
            {
                case"fake":
                    logger.LogInformation("Using fake store - skipping receipt validation");
                    await ApplyPurchaseRewardsFromConfiguration(context, gameApiClient, productId);
                    break;
                case "googleplay":
                    await RedeemGooglePlayPurchase(context, gameApiClient, productId, receiptData.PayLoad, localCost, localCurrency);
                    break;
                case "appleappstore":
                    await RedeemAppleAppStorePurchase(context, gameApiClient, productId, receiptData.PayLoad, localCost, localCurrency);
                    break;
                default:
                    throw new ArgumentException($"Unsupported store type: {store}");
            }
        }

        private async Task ApplyPurchaseRewardsFromConfiguration(IExecutionContext context, IGameApiClient gameApiClient, string productId)
        {
            try
            {
                var configResponse = await gameApiClient.EconomyConfiguration.GetPlayerConfigurationAsync(
                    context,
                    context.AccessToken,
                    context.ProjectId,
                    context.PlayerId!);

                var realMoneyPurchase = GetRealMoneyPurchaseFromConfig(configResponse.Data.Results, productId);

                if (realMoneyPurchase?.Rewards != null)
                {
                    await DistributeConfiguredRewards(context, gameApiClient, configResponse.Data.Results, realMoneyPurchase.Rewards);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Failed to grant rewards for product {productId}");
                throw;
            }
        }

        private async Task DistributeConfiguredRewards(
            IExecutionContext context,
            IGameApiClient gameApiClient,
            List<PlayerConfigurationResponseResultsInner> configResults,
            List<Reward> rewards)
        {
            foreach (var reward in rewards)
            {
                string resourceId = reward.ResourceId;
                int amount = (int)reward.Amount;

                logger.LogInformation($"Processing reward: {resourceId}, Amount: {amount}");

                string resourceType = playerEconomyService.GetResourceType(configResults, resourceId);
                await playerEconomyService.GrantResourceReward(context, gameApiClient, resourceType, resourceId, amount);
            }
        }

        private RealMoneyPurchaseResource? GetRealMoneyPurchaseFromConfig(
            List<PlayerConfigurationResponseResultsInner> results,
            string productId)
        {
            foreach (var result in results)
            {
                if (result.ActualInstance is RealMoneyPurchaseResource purchase && purchase.Id == productId)
                {
                    return purchase;
                }
            }

            logger.LogError($"Real money purchase not found: {productId}");
            throw new InvalidOperationException($"Real money purchase not found: {productId}");
        }

        private async Task RedeemAppleAppStorePurchase(IExecutionContext context, IGameApiClient gameApiClient, string productId,
            string applePayload, double localCost, string localCurrency)
        {
            if (string.IsNullOrWhiteSpace(applePayload))
            {
                throw new ArgumentException("Apple receipt payload is empty.", nameof(applePayload));
            }

            var appleRequest = new PlayerPurchaseAppleappstoreRequest
            {
                Id = productId,
                Receipt = applePayload,
                LocalCost = (int)(localCost * 100),
                LocalCurrency = localCurrency
            };

            var purchaseResult = await gameApiClient.EconomyPurchases.RedeemAppleAppStorePurchaseAsync(
                context,
                context.AccessToken,
                context.ProjectId,
                context.PlayerId!,
                appleRequest);

            foreach (var currency in purchaseResult.Data.Rewards.Currency)
            {
                logger.LogInformation($"Granted {currency.Amount} {currency.Id}");
            }

            foreach (var item in purchaseResult.Data.Rewards.Inventory)
            {
                logger.LogInformation($"Granted {item.Amount} {item.Id}");
            }
        }

        private async Task RedeemGooglePlayPurchase(IExecutionContext context, IGameApiClient gameApiClient, string productId,
            string googlePayload, double localCost, string localCurrency)
        {
            var googleReceipt = JsonConvert.DeserializeAnonymousType(googlePayload, new { json = "", signature = "" })
                ?? throw new JsonException("Faild  to parse Google receipt payload.");

            if(string.IsNullOrWhiteSpace(googleReceipt.json) || string.IsNullOrWhiteSpace(googleReceipt.signature))
            {
                throw new JsonException("Google payload missing json/signature.");
            }

            var googleRequest = new PlayerPurchaseGoogleplaystoreRequest
            {
                Id = productId,
                PurchaseData = googleReceipt.json,
                PurchaseDataSignature = googleReceipt.signature,
                LocalCost = (int)(localCost * 100),
                LocalCurrency = localCurrency
            };

            var purchaseResult = await gameApiClient.EconomyPurchases.RedeemGooglePlayPurchaseAsync(
                context,
                context.AccessToken,
                context.ProjectId,
                context.PlayerId!,
                googleRequest);

            foreach (var currency in purchaseResult.Data.Rewards.Currency)
            {
                logger.LogInformation($"Granted {currency.Amount} {currency.Id}");
            }

            foreach (var item in purchaseResult.Data.Rewards.Inventory)
            {
                logger.LogInformation($"Granted {item.Amount} {item.Id}");
            }
        }


    }
}
