using System;

public class MarketPriceModel
{
    public Guid MarketPriceSnapshotId { get; set; }
    public string FishDefinitionKey { get; set; } = string.Empty;
    public int CurrentBuyPriceMinorUnits { get; set; }
    public int CurrentSellPriceMinorUnits { get; set; }
    public Season PeakSeason { get; set; }
    public int Difficulty { get; set; }
    public DateTime RetrievedUtc { get; set; }
}