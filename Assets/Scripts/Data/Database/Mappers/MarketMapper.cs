public class MarketMapper
{
    public static MarketPriceModel ToModel(MarketPriceSnapshotEntity entity)
    {
        return new MarketPriceModel
        {
            MarketPriceSnapshotId = entity.MarketPriceSnapshotId,
            FishDefinitionKey = entity.FishDefinitionKey,
            CurrentBuyPriceMinorUnits = entity.CurrentBuyPriceMinorUnits,
            CurrentSellPriceMinorUnits = entity.CurrentSellPriceMinorUnits,
            RetrievedUtc = entity.RetrievedUtc
        };
    }
}