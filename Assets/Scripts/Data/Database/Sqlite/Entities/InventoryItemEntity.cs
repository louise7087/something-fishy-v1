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

/// <summary>
/// Represents the persisted Inventory item row stored in SQLite.
/// </summary>
/// <remarks>
/// Responsibilities:
/// - Store persisted inventory item data.
/// - Represent item identity, quantity, and ownership/location.
/// - Support loading and saving inventory state in SQLite.
///
/// This type is a pure persistence model and must remain separate from
/// runtime/domain inventory behavior (mapping to/from domain models
/// should be done in repository or mapper layers).
/// </remarks>
public sealed class InventoryItemEntity
{
    public Guid InventoryItemId { get; set; } = default!;
    public Guid PlayerId { get; set; } = default!;
    public string ItemDefinitionKey { get; set; } = default!;
    public int Quantity { get; set; }
    public int SlotIndex { get; set; }
    public string? SourceTag { get; set; }
    public DateTime CreatedUtcTicks { get; set; }
    public DateTime UpdatedUtcTicks { get; set; }
}