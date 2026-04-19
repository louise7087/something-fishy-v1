using UnityEditor;
using System;
using System.Threading.Tasks;

public class SellFishToMarketService
{
    private readonly MarketPriceRepository _marketPriceRepository;

    public SellFishToMarketService(MarketPriceRepository marketPriceRepository)
    {
        _marketPriceRepository = marketPriceRepository;
    }

    public async Task<int> loadFishPrice(string fishDefinitionKey)
    {
        var marketSnapshot = await _marketPriceRepository.GetByFishDefinitionKey(fishDefinitionKey);

        return marketSnapshot.CurrentSellPriceMinorUnits;
    }
}