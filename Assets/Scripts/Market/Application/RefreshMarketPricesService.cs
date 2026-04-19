using System;
using System.Collections.Generic;
using UnityEngine;

public class RefreshMarketPricesService
{
    private readonly MarketPriceRepository _marketPriceRepository;
    public RefreshMarketPricesService(MarketPriceRepository marketPriceRepository)
    {
        _marketPriceRepository = marketPriceRepository;
    }
    public async void RefreshMarketPricesAsync(Season season)
    {
        var marketPrices = await _marketPriceRepository.GetAllAsync();
        foreach (var marketPrice in marketPrices)
        {
            // Simulate price fluctuation by randomly adjusting the price within a range
            float demandMultiplier = GetSimulatedDemandMultiplier(marketPrice.FishDefinitionKey);
            float seasonalMultiplier = GetSeasonalMultiplier(marketPrice.FishDefinitionKey, season);
            marketPrice.CurrentBuyPriceMinorUnits = Mathf.RoundToInt(marketPrice.CurrentBuyPriceMinorUnits * demandMultiplier * seasonalMultiplier);
            // Ensure the price does not drop below a certain threshold
            if (marketPrice.CurrentBuyPriceMinorUnits < 1)
            {
                marketPrice.CurrentBuyPriceMinorUnits = 1;
            }
            await _marketPriceRepository.UpdateAsync(marketPrice);
        }
    }
    private float GetSimulatedDemandMultiplier(string fishId)
    {
        float noise = Mathf.PerlinNoise(
            Time.time * 120f,
            fishId.GetHashCode() * 0.01f
        );

        return Mathf.Lerp(0.8f, 1.2f, noise);
    }

    private float GetSeasonalMultiplier(string fishId, Season season)
    {
        switch (season)
        {
            case Season.SPRING:
                return fishId == "salmon" ? 1.2f : 1.0f;

            case Season.SUMMER:
                return fishId == "tuna" ? 1.3f : 1.0f;

            case Season.FALL:
                return fishId == "carp" ? 1.2f : 1.0f;

            case Season.WINTER:
                return fishId == "cod" ? 1.3f : 1.0f;

            default:
                return 1.0f;
        }
    }
}