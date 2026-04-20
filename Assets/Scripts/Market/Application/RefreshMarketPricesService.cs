using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class RefreshMarketPricesService
{
    private readonly MarketPriceRepository _marketPriceRepository;
    private const string LAST_MARKET_REFRESH_KEY = "lastMarketRefreshUtc";
    private static readonly TimeSpan RefreshInterval = TimeSpan.FromMinutes(1);
    public RefreshMarketPricesService(MarketPriceRepository marketPriceRepository)
    {
        _marketPriceRepository = marketPriceRepository;
    }
    public async Task RefreshMarketPricesAsync(Season season)
    {
        var marketPrices = await _marketPriceRepository.GetAllAsync();
        foreach (var marketPrice in marketPrices)
        {
            // Simulate price fluctuation by randomly adjusting the price within a range
            float demandMultiplier = GetSimulatedDemandMultiplier(marketPrice.FishDefinitionKey);
            float seasonalMultiplier = GetSeasonalMultiplier(marketPrice.PeakSeason, season);
            marketPrice.CurrentSellPriceMinorUnits = Mathf.RoundToInt(marketPrice.CurrentSellPriceMinorUnits * demandMultiplier * seasonalMultiplier);
            // Ensure the price does not drop below a certain threshold
            if (marketPrice.CurrentSellPriceMinorUnits < 1)
            {
                marketPrice.CurrentSellPriceMinorUnits = 1;
            }
            await _marketPriceRepository.UpdateAsync(marketPrice);
        }
    }

    public async Task RefreshOnIntervalAsync(Season season)
    {
        string lastRefreshText = PlayerPrefs.GetString(LAST_MARKET_REFRESH_KEY, string.Empty);

        if (DateTime.TryParse(lastRefreshText, out var lastRefresh))
        {
            if (DateTime.UtcNow - lastRefresh < RefreshInterval)
            {
                return;
            }
        }

        await RefreshMarketPricesAsync(season);

        PlayerPrefs.SetString(LAST_MARKET_REFRESH_KEY, DateTime.UtcNow.ToString("O"));
        PlayerPrefs.Save();
    }

    private float GetSimulatedDemandMultiplier(string fishId)
    {
        float noise = Mathf.PerlinNoise(
            Time.time * 120f,
            fishId.GetHashCode() * 0.01f
        );

        return Mathf.Lerp(0.8f, 1.2f, noise);
    }

    private float GetSeasonalMultiplier(Season currentSeasonIndex, Season peakSeasonIndex)
    {
        if (currentSeasonIndex == peakSeasonIndex)
        {
            return 1.25f;
        }

        return 0.9f;
    }
}