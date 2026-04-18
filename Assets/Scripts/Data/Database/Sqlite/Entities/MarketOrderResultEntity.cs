using System;

public sealed class MarketOrderResultEntity
{
    public Guid OrderResultId { get; set; }
    public Guid PlayerId { get; set; }
    public string OrderType { get; set; } = string.Empty; // SellFish, BuyUpgrade, BuySlot
    public string FishDefinitionKey { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public int UnitPriceMinorUnits { get; set; }
    public int TotalMinorUnits { get; set; }
    public string ExternalOrderId { get; set; } = string.Empty; // for correlating with external market/order systems
    public DateTime ProcessedUtc { get; set; }
}
