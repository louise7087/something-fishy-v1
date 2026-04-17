// ============================================================
// File: UpgradeOwnershipEntity.cs
// Purpose:
// Represents the database row/entity used to persist player
// upgrade ownership or upgrade progression state.
//
// Responsibilities:
// - Store persisted upgrade data
// - Represent owned or unlocked upgrades in SQLite
// - Support loading and saving upgrade progression
//
// Notes:
// - Persistence model only
// - Upgrade effects and purchase rules belong in domain/services
// ============================================================

using System;

public sealed class UpgradeOwnershipEntity
{
    public Guid UpgradeOwnershipId { get; set; } = default!;
    public Guid PlayerId { get; set; } = default!;
    public string UpgradeDefinitionKey { get; set; } = string.Empty;
    public int Level { get; set; }
    public DateTime PurchasedUtcTicks { get; set; }

    public PlayerEntity Player { get; set; }
}