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

/// <summary>
/// Represents the persisted wallet row stored in SQLite.
/// </summary>
/// <remarks>
/// Responsibilities:
/// - Store persisted wallet data.
/// - Represent wallet identity, balance, and ownership.
/// - Support loading and saving wallet state in SQLite.
///
/// This type is a pure persistence model and must remain separate from
/// runtime/domain wallet behavior (mapping to/from domain models
/// should be done in repository or mapper layers).
/// </remarks>
public sealed class WalletEntity
{
    public Guid WalletId { get; set; }
    public Guid PlayerId { get; set; }
    public int BalanceMinorUnits { get; set; }
    public DateTime UpdatedUtc { get; set; }

    public PlayerEntity Player { get; set; } = null!;
}