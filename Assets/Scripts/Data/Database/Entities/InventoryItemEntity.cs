using System;

public sealed class InventoryItemEntity
{
    public Guid InventoryItemId { get; set; } = default!;
    public Guid PlayerId { get; set; } = default!;
    public string ItemDefinitionKey { get; set; } = default!;
    public int Quantity { get; set; }
    public int SlotIndex { get; set; }
    public string? SourceTag { get; set; }
    public DateTime CreatedUtc { get; set; }
    public DateTime UpdatedUtc { get; set; }

    public PlayerEntity Player { get; set; } = null!;
}