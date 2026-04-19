// ============================================================
// File: WalletEntity.cs
// Purpose:
// Represents the database row/entity used to persist a player's wallet
// and their associated data.
//
// Responsibilities:
// - Store persisted wallet data
// - Represent player identity, progression, and associated entities
// - Support loading and saving player state in SQLite
//
// Notes:
// - Persistence model only
// - Keep storage structure separate from domain player logic
// ============================================================

using System;

public sealed class WalletEntity
{
    public Guid WalletId { get; set; }
    public Guid PlayerId { get; set; }
    public int BalanceMinorUnits { get; set; }
    public DateTime UpdatedUtc { get; set; }

    public PlayerEntity Player { get; set; } = null!;
}