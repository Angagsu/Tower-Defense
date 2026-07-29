using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.Services.CloudCode.Apis;
using Unity.Services.CloudCode.Core;
using Unity.Services.CloudCode.Shared;
using Unity.Services.Economy.Model;

namespace HayguardRiseOfHighlandCloud
{
    public class PlayerEconomyService
    {
        public const string GOLD_CURRENCY_KEY = "GOLD";
        public const string CARD_CURRENCY_KEY = "CARD";
        private const int startingCardAmount = 5;

        private readonly ILogger<PlayerEconomyService> logger;



        public PlayerEconomyService(ILogger<PlayerEconomyService> logger)
        {
            this.logger = logger;
        }



    
        private async Task<int> GetPlayerGold(IExecutionContext context, IGameApiClient gameApiClient)
        {
            return await GetCurrencyAmount(context, gameApiClient, GOLD_CURRENCY_KEY);
        }

        private async Task<int> GetCurrencyAmount(IExecutionContext context, IGameApiClient gameApiClient, string currencyKey)
        {
            try
            {
                var playerCurrenciesData = await gameApiClient.EconomyCurrencies.GetPlayerCurrenciesAsync(
                    context,
                    context.AccessToken!,
                    context.ProjectId!,
                    context.PlayerId!
                    );

                CurrencyBalanceResponse? targetCurrency = playerCurrenciesData.Data.Results.FirstOrDefault(currency => currency.CurrencyId == currencyKey);

                if (targetCurrency != null)
                {
                    return (int)targetCurrency.Balance;
                }
                else
                {
                    throw new Exception($"Currency with key '{currencyKey}' not found.");
                }

            }
            catch (ApiException ex)
            {
                throw new Exception($"Faild to get currency {currencyKey} for player {context.PlayerId}. Error: {ex.Message}");
            }
        }

        public async Task AddCurrency(IExecutionContext context, IGameApiClient gameApiClient, string resourceId, int amount)
        {
            try
            {
                // Fetch player currencies
                var playerCurrenciesData = await gameApiClient.EconomyCurrencies.GetPlayerCurrenciesAsync(
                    context,
                    context.AccessToken!,
                    context.ProjectId!,
                    context.PlayerId!
                );

                // Find target currency (if any)
                CurrencyBalanceResponse? targetCurrency = playerCurrenciesData.Data.Results
                    .FirstOrDefault(c => c.CurrencyId == resourceId);

                // Current balance (0 if currency not yet present)
                int currentBalance = targetCurrency != null ? (int)targetCurrency.Balance : 0;


                int newBalance = currentBalance + amount;

                // Build request to set/update the currency balance
                var currencyRequest = new CurrencyBalanceRequest { Balance = newBalance };

                // Update the player's currency balance
                await gameApiClient.EconomyCurrencies.SetPlayerCurrencyBalanceAsync(
                    context,
                    context.AccessToken,
                    context.ProjectId,
                    context.PlayerId!,
                    resourceId,
                    currencyRequest
                );

                logger.LogInformation($"Updated currency '{resourceId}' for player {context.PlayerId}: {currentBalance} -> {newBalance}");
            }
            catch (ApiException ex)
            {
                logger.LogError(ex, $"Failed to add currency {resourceId} for player {context.PlayerId}. Error: {ex.Message}");
                throw new Exception($"Failed to add currency {resourceId} for player {context.PlayerId}. Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Unexpected error while adding currency {resourceId} for player {context.PlayerId}");
                throw;
            }
        }


        private async Task<int> GetPlayerCard(IExecutionContext context, IGameApiClient gameApiClient)
        {
            return await GetInventoryItemAmount(context, gameApiClient, CARD_CURRENCY_KEY);
        }

        private async Task<int> GetInventoryItemAmount(IExecutionContext context, IGameApiClient gameApiClient, string itemKey)
        {
            try
            {
                var playerInventoryResponse = await gameApiClient.EconomyInventory.GetPlayerInventoryAsync(
                    context,
                    context.AccessToken!,
                    context.ProjectId!,
                    context.PlayerId!,
                    inventoryItemIds: new List<string> { itemKey }
                    );

                InventoryResponse? item = playerInventoryResponse.Data.Results.FirstOrDefault();

                if (item == null)
                {
                    logger.LogInformation($"Inventory item with key {itemKey} not found for player {context.PlayerId}");
                    return 0;
                }

                if(!TryParseInventoryItemAmount(item, out int amount))
                {
                    return 0;
                }

                return amount;
            }
            catch (ApiException ex)
            {
                throw new Exception($"Failed to get inventory item {itemKey} for player {context.PlayerId}. Error: {ex.Message}");
            }

        }

        private bool TryParseInventoryItemAmount(InventoryResponse item, out int amount)
        {
            amount = 0;

            if (item.InstanceData == null)
            {
                logger.LogInformation($"Inventory item '{item.InventoryItemId}' instance data is null.");
                return false;
            }

            try
            {
                string json = $"{item.InstanceData}";
                var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

                if(data != null && data.TryGetValue("amount", out var amountObj))
                {
                    if (int.TryParse(amountObj.ToString(), out amount))
                    {
                        return true;
                    }

                    logger.LogWarning($"Amount value '{amountObj}' for '{item.InventoryItemId}' is not a valid integer.");
                    return false;
                }

                logger.LogWarning($"Instance data for '{item.InventoryItemId}' does not contain an 'amount' property.");
                return false;
            }
            catch (Exception ex)
            {
                logger.LogWarning($"Failed to parse inventory item amount for '{item.InventoryItemId}'. Error: {ex.Message}");
                return false;
            }
        }

        
        private async Task InitializeInventory(IExecutionContext context, IGameApiClient gameApiClient)
        {
            var startingItems = new Dictionary<string, int>
            {
                { CARD_CURRENCY_KEY, startingCardAmount }
            };

            foreach (var item in startingItems)
            {
                try
                {
                    await AddNewInventoryItem(context, gameApiClient, item.Key, item.Value);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, $"Failed to initialize inventory item {item.Key}");
                }
                
            }
        }

        public async Task AddNewInventoryItem(IExecutionContext context, IGameApiClient gameApiClient, string itemId, int amount)
        {
            var instanceData = new Dictionary<string, object>
            {
                { "amount", amount }
            };
            await AddNewInventoryItem(context, gameApiClient, itemId, instanceData);
        }

        public async Task AddNewInventoryItem(IExecutionContext context,IGameApiClient gameApiClient,string itemId,Dictionary<string, object> instanceData)
        {
            var inventoryRequest = new AddInventoryRequest(itemId, instanceData: instanceData);

            try
            {
                await gameApiClient.EconomyInventory.AddInventoryItemAsync(
                    context,
                    context.AccessToken,
                    context.ProjectId,
                    context.PlayerId ?? throw new InvalidOperationException("PlayerId is null"),
                    inventoryRequest
                    );
            }
            catch (ApiException ex)
            {
                logger.LogError($"Failed to add inventory item {itemId} for player {context.PlayerId}. Error: {ex.Message}");
            }
        }


        [CloudCodeFunction("GetPlayerEconomyData")]
        public async Task<PlayerEconomyData> GetPlayerEconomyData(IExecutionContext context, IGameApiClient gameApiClient)
        {
            try
            {
                var economyData = new PlayerEconomyData();

                int goldAmount = await GetPlayerGold(context, gameApiClient);
                economyData.Currencies[GOLD_CURRENCY_KEY] = goldAmount;

                economyData.ItemInventory = await GetPlayerInventoryItemAmountMap(context, gameApiClient);

                return economyData;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Failed to get player economy data for player {context.PlayerId}");
                throw new Exception($"Failed to get player economy data for player {context.PlayerId}. Error: {ex.Message}");
            }
        }

        public async Task<PlayerEconomyData> InitializeNewPlayerEconomy(IExecutionContext context, IGameApiClient gameApiClient)
        {
            await InitializeInventory(context, gameApiClient);
            return await GetPlayerEconomyData(context, gameApiClient);
        }

        private async Task<Dictionary<string, int>> GetPlayerInventoryItemAmountMap(
            IExecutionContext context,
            IGameApiClient gameApiClient,
            params string[]? inventoryItemIds)
        {
            var items = await GetPlayerInventory(context, gameApiClient, inventoryItemsIds: inventoryItemIds);

            return items
                .Where(item => !string.IsNullOrEmpty(item.InventoryItemId))
                .ToDictionary(
                item => item.InventoryItemId!,
                item => GetInventoryItemCustomData<int?>(item, "amount") ?? 1
                );
        }

        private async Task<List<InventoryResponse>> GetPlayerInventory(IExecutionContext context, IGameApiClient gameApiClient,
            int? limit = null, params string[]? inventoryItemsIds)
        {
            try
            {
                List<string>? ids = inventoryItemsIds?.Length > 0
                    ? inventoryItemsIds.ToList()
                    : null;

                var playerInventory = await gameApiClient.EconomyInventory.GetPlayerInventoryAsync(
                    context,
                    context.AccessToken,
                    context.ProjectId,
                    context.PlayerId!,
                    inventoryItemIds: ids,
                    limit: limit
                    );

                return playerInventory.Data.Results;
            }
            catch (ApiException ex)
            {
                logger.LogError($"Failed to get player inventory for player {context.PlayerId}. Error: {ex.Message}");
                throw new Exception($"Failed to get player inventory for player {context.PlayerId}. Error: {ex.Message}");
            }
        }

        private T? GetInventoryItemCustomData<T>(InventoryResponse item, string key)
        {
            if (item?.InstanceData == null) return default;

            try
            {
                var jObject = item.InstanceData as Newtonsoft.Json.Linq.JObject 
                    ?? Newtonsoft.Json.Linq.JObject.Parse(item.InstanceData?.ToString() ?? "{}");

                var token = jObject[key];

                if(token != null)
                {
                    return token.ToObject<T>();
                }
            }
            catch (Exception ex)
            {
                logger.LogWarning($"Faild to get {key} from item {item.InventoryItemId}: {ex.Message}");
            }

            return default;
        }

        public async Task CleanUpNullOrZeroAmountItems(IExecutionContext context, IGameApiClient gameApiClient, string itemKey)
        {
            try
            {
                var items = await GetPlayerInventory(context, gameApiClient, inventoryItemsIds: new string[] { itemKey });

                var itemsToDelete = new List<string>();

                foreach (var item in items)
                {
                    if(string.IsNullOrEmpty(item.PlayersInventoryItemId)) continue;

                    if(item.InstanceData == null)
                    {
                        itemsToDelete.Add(item.PlayersInventoryItemId);
                        logger.LogInformation($"Found {itemKey} with null instance data: {item.PlayersInventoryItemId}");
                        continue;
                    }

                    if (!TryParseInventoryItemAmount(item, out int amount))
                    {
                        continue;
                    }

                    if(amount <= 0)
                    {
                        itemsToDelete.Add(item.PlayersInventoryItemId);
                    }
                }

                foreach (var itemId in itemsToDelete)
                {
                    await DeleteInventoryItem(context, gameApiClient, itemId);
                    logger.LogInformation($"Deleted zero-amount {itemId}");
                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Failed to clean up zero-amount {itemKey} for player {context.PlayerId}");
            }
        }

        public async Task DeleteInventoryItem(IExecutionContext context, IGameApiClient gameApiClient, string itemId)
        {
            await gameApiClient.EconomyInventory.DeleteInventoryItemAsync(
                context,
                context.AccessToken,
                context.ProjectId,
                context.PlayerId ?? throw new InvalidOperationException("PlayerId is null"),
                itemId
                );
        }

        public async Task AddOrUpdateInventoryItemAmount(IExecutionContext context, IGameApiClient gameApiClient, string itemKey,
            int amountToAdd, Dictionary<string, object>? customData = null)
        {
            var inventoryItems = await GetPlayerInventory(context, gameApiClient, inventoryItemsIds: new string[] { itemKey });
            InventoryResponse? existingItem = inventoryItems.FirstOrDefault(item => !string.IsNullOrEmpty(item.PlayersInventoryItemId));
            bool itemExistsInInventory = existingItem != null;

            int totalAmount = amountToAdd;

            if (itemExistsInInventory)
            {
                TryParseInventoryItemAmount(existingItem!, out int curentAmount);

                totalAmount = curentAmount + amountToAdd;
            }

            var instanceData = new Dictionary<string, object>
            {
                { "amount", totalAmount }
            };

            if(customData != null)
            {
                foreach (var kvp in customData)
                {
                    instanceData[kvp.Key] = kvp.Value;
                }
            }

            if (itemExistsInInventory)
            {
                var updateRequest = new InventoryRequestUpdate(instanceData: instanceData);
                await gameApiClient.EconomyInventory.UpdateInventoryItemAsync(context, context.AccessToken, context.ProjectId, context.PlayerId!,
                    existingItem!.PlayersInventoryItemId!, updateRequest);
            }
            else
            {
                await AddNewInventoryItem(context, gameApiClient, itemKey, instanceData);
            }
        }

        public string GetResourceType(List<PlayerConfigurationResponseResultsInner> results, string resourceId)
        {
            foreach (var result in results)
            {
                switch (result.ActualInstance)
                {   
                    case CurrencyResource currency when currency.Id == resourceId:
                        return "CURRENCY";
                    case InventoryItemResource item when item.Id == resourceId:
                        return "INVENTORY_ITEM";
                    case RealMoneyPurchaseResource purchase when purchase.Id == resourceId:
                        return "REAL_MONEY_PURCHASE";
                    case VirtualPurchaseResource virtualPurchase when virtualPurchase.Id == resourceId:
                        return "VIRTUAL_PURCHASE";
                }
            }
            return "UNKNOWN";
        }

        public async Task GrantResourceReward(
            IExecutionContext context,
            IGameApiClient gameApiClient,
            string resourceType,
            string resourceId,
            int amount)
        {
            switch (resourceType)
            {
                case "CURRENCY":
                    await AddCurrency(context, gameApiClient, resourceId, amount);
                    logger.LogInformation($"Added currency: {resourceId}, Amount: {amount}");
                    break;
                case "INVENTORY_ITEM":
                    await AddOrUpdateInventoryItemAmount(context, gameApiClient, resourceId, amount);
                    logger.LogInformation($"Added inventory item: {resourceId}, Amount: {amount}");
                    break;
                default:
                    logger.LogWarning($"Unknown resource type for reward: {resourceType}");
                    break;
            }
        }
        
    }
}
