using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Services.CloudCode.Apis;
using Unity.Services.CloudCode.Core;

namespace HayguardRiseOfHighlandCloud
{
    public class ParsedTokenData
    {
        public long Timestamp { get; init; }
        public string InstanceId { get; init; } = "";
        public string InstanceName { get; init; } = "";
        public string AdNetwork { get; init; } = "";
        public string RewardName { get; init; } = "";
        public int RewardAmount { get; init; }
    }

    public class AdRewardsService
    {
        private const int MAX_TOKEN_AGE_MINUTES = 30;
        private const int MAX_FUTURE_TO_IERANCE_SECONDS = 60;
        private const int MAX_AD_REWARD_AMOUNT = 1000;
        private const string EXPECTED_AD_REWARD_CURRENCY_ID = "GOLD";
        private const string LAST_AD_TOKEN_KEY = "LAST_AD_TOKEN";
        private const int MIN_AD_INTERVAL_SECONDS = 10;

        private readonly ILogger<AdRewardsService> logger;
        private readonly PlayerEconomyService playerEconomyService;
        private readonly PlayerDataService playerDataService;

        public AdRewardsService(
            ILogger<AdRewardsService> logger,
            PlayerEconomyService playerEconomyService,
            PlayerDataService playerDataService)
        {
            this.logger = logger;
            this.playerEconomyService = playerEconomyService;
            this.playerDataService = playerDataService;
        }

        [CloudCodeFunction("HandleGrantVideoAdReward")]
        public async Task<PlayerEconomyData> HandleGrantVideoAdReward(IExecutionContext context, IGameApiClient gameApiClient, string adToken)
        {
            try
            {
                ParsedTokenData tokenData = ParseToken(adToken);

                ValidateTokenData(tokenData);

                await ValidateTokenUsage(context, gameApiClient, adToken);
               
                await playerEconomyService.AddCurrency(context, gameApiClient, PlayerEconomyService.GOLD_CURRENCY_KEY, tokenData.RewardAmount);
               
                await StoreLastAdToken(context, gameApiClient, adToken);

                logger.LogInformation($"Successfully granted ad reward: {tokenData.RewardName} x{tokenData.RewardAmount} from {tokenData.AdNetwork}");

                return await playerEconomyService.GetPlayerEconomyData(context, gameApiClient)
                    ?? throw new InvalidOperationException("faild to get player economy data");
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error granting reward");
                throw;
            }
        }

        private async Task StoreLastAdToken(IExecutionContext context, IGameApiClient gameApiClient, string adToken)
        {
            await playerDataService.SaveData(context, gameApiClient, LAST_AD_TOKEN_KEY, adToken);
        }

        private async Task ValidateTokenUsage(IExecutionContext context, IGameApiClient gameApiClient, string adToken)
        {
            try
            {
                var (success, previousTokenObj) = await playerDataService.TryGetData(context, gameApiClient, LAST_AD_TOKEN_KEY);

                string? previousToken = null;

                if (success && previousTokenObj != null)
                {
                    previousToken = $"{previousTokenObj}";
                }

                if(HasTokenBeenUsed(adToken, previousToken))
                {
                    throw new UnauthorizedAccessException("Ad token is duplicate of last used. Reward denied");
                }

                if (!HasSufficientAdIntervalElapsed(previousToken))
                {
                    throw new UnauthorizedAccessException("Ad rewarded too quickly. Reward denied");
                }
            }
            catch (Exception ex) when( !(ex is UnauthorizedAccessException)) 
            {
                logger.LogError(ex, "Error validating token usage");
                throw new UnauthorizedAccessException("Unable to validate token usage");
            }
        }

        private bool HasSufficientAdIntervalElapsed(string? previousToken)
        {
            if (string.IsNullOrEmpty(previousToken))
            {
                return true;
            }

            try
            {
                ParsedTokenData previousTokenData = ParseToken(previousToken);

                DateTime previousTimestamp = new DateTime(previousTokenData.Timestamp, DateTimeKind.Utc);
                TimeSpan timeBetweenAds = DateTime.UtcNow - previousTimestamp;

                if (timeBetweenAds.TotalSeconds < MIN_AD_INTERVAL_SECONDS)
                {
                    logger.LogWarning("Ad rewarded too quickly. Time since last ad: {TimeBetweenAds} seconds", timeBetweenAds.TotalSeconds);
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error parsing previous token for timing validation. Allowing reward");
                return true; 
            }
        }

        private bool HasTokenBeenUsed(string adToken, string? previousToken)
        {
            if(!string.IsNullOrEmpty(previousToken) && adToken == previousToken)
            {
                logger.LogWarning($"Duplicate ad token detected: {adToken}");
                return true;
            }
            return false;
        }

        private void ValidateTokenData(ParsedTokenData data)
        {
            DateTime timestamp;

            try
            {
                timestamp = new DateTime(data.Timestamp, DateTimeKind.Utc);
            }
            catch (ArgumentOutOfRangeException)
            {
                throw new UnauthorizedAccessException("Invalid timestamp in token");
            }

            DateTime now = DateTime.UtcNow;
            TimeSpan age = now - timestamp;

            if (age.TotalMinutes > MAX_TOKEN_AGE_MINUTES)
            {
                throw new UnauthorizedAccessException("Token is too old");
            }

            if (timestamp > now.AddSeconds(MAX_FUTURE_TO_IERANCE_SECONDS))
            {
                throw new UnauthorizedAccessException("Token timestamp is too far in the future");
            }

            if (string.IsNullOrEmpty(data.InstanceId))
            {
                throw new UnauthorizedAccessException("Instance ID cannot be empty");
            }

            if (data.RewardName != EXPECTED_AD_REWARD_CURRENCY_ID)
            {
                throw new UnauthorizedAccessException($"Invalid reward name. Expected '{EXPECTED_AD_REWARD_CURRENCY_ID}', got '{data.RewardName}'");
            }

            if(data.RewardAmount <= 0)
            {
                throw new UnauthorizedAccessException($"Invalid reward amount. Must be positive, got {data.RewardAmount}");
            }

            if(data.RewardAmount > MAX_AD_REWARD_AMOUNT)
            {
                throw new UnauthorizedAccessException($"Invalid reward amount. Expected under {MAX_AD_REWARD_AMOUNT}, got {data.RewardAmount}");
            }
        }

        private ParsedTokenData ParseToken(string adToken)
        {
            if (string.IsNullOrEmpty(adToken))
            {
                throw new UnauthorizedAccessException("Token is null or empty");
            }

            try
            {
                var tokenData = JsonConvert.DeserializeObject<ParsedTokenData>(adToken);

                if (tokenData == null)
                {
                    throw new UnauthorizedAccessException("Failed to parse token");
                }

                return tokenData;
            }
            catch (JsonException ex)
            {
                logger.LogError($"Failed to parse token: Token {adToken}, Error {ex.Message}");
                throw new UnauthorizedAccessException("Invalid token format");
            }
        }
    }
}
