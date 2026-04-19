using UnityEditor;
using System;
using System.Threading.Tasks;

public class SellFishToMarketService
{
    private readonly MarketPriceRepository _marketPriceRepository;
    private readonly WalletRepository _walletRepository;
    private readonly InventoryRepository _inventoryRepository;

    public SellFishToMarketService(MarketPriceRepository marketPriceRepository, InventoryRepository inventoryRepository, WalletRepository walletRepository)
    {
        _marketPriceRepository = marketPriceRepository;
        _inventoryRepository = inventoryRepository;
        _walletRepository = walletRepository;
    }

    public async Task<int> loadFishPrice(string fishDefinitionKey)
    {
        var marketSnapshot = await _marketPriceRepository.GetByFishDefinitionKey(fishDefinitionKey);

        return marketSnapshot.CurrentSellPriceMinorUnits;
    }
}