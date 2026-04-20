public class MarketMapper
{
    public static MarketPriceModel ToModel(MarketPriceSnapshotEntity entity)
    {
        return new MarketPriceModel
        {
            MarketPriceSnapshotId = entity.MarketPriceSnapshotId,
            FishDefinitionKey = entity.FishDefinitionKey,
            CurrentBuyPriceMinorUnits = entity.CurrentSellPriceMinorUnits,
            CurrentSellPriceMinorUnits = entity.CurrentSellPriceMinorUnits,
            PeakSeason = entity.PeakSeason,
            Difficulty = entity.Difficulty,
            RetrievedUtc = entity.RetrievedUtc
        };
    }
}