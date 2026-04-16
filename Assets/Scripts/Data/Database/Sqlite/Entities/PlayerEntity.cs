// ============================================================
// File: PlayerEntity.cs
// Purpose:
// Represents the database row/entity used to persist a player
// and their associated data.
//
// Responsibilities:
// - Store persisted player data
// - Represent player identity, progression, and associated entities
// - Support loading and saving player state in SQLite
//
// Notes:
// - Persistence model only
// - Keep storage structure separate from domain player logic
// ============================================================

using System;
using System.Collections.Generic;

/// <summary>
/// Represents the persisted player row stored in SQLite.
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
public sealed class PlayerEntity
{
    public Guid PlayerId { get; set; }
    public string DisplayName { get; set; } = string.Empty;
    public int ProgressionLevel { get; set; }
    public string CurrentAreaKey { get; set; } = "starter_area";
    public string EquippedToolKey { get; set; } = "basic_rod";
    public DateTime CreatedUtc { get; set; }
    public DateTime UpdatedUtc { get; set; }

    public WalletEntity? Wallet { get; set; }
    public List<InventoryItemEntity> InventoryItems { get; set; } = new ();
    public List<UpgradeOwnershipEntity> UpgradesOwnership { get; set; } = new();
    public List<UnlockEntity> Unlocks { get; set; } = new();
    public List<TransactionEntity> Transactions { get; set; }
}