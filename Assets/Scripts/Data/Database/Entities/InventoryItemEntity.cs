// ============================================================
// File: InventoryItemEntity.cs
// Purpose:
// Represents the database row/entity used to persist an item
// stored in a player or container inventory.
//
// Responsibilities:
// - Store persisted inventory item data
// - Represent item identity, quantity, and ownership/location
// - Support loading and saving inventory state in SQLite
//
// Notes:
// - Persistence model only
// - Keep storage structure separate from domain inventory logic
// ============================================================

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