// ============================================================
// File: UnlockEntity.cs
// Purpose:
// Represents the database row/entity used to persist player
// progression unlocks such as world areas or gated content.
//
// Responsibilities:
// - Store persisted unlock state
// - Represent which progression gates have been unlocked
// - Support loading and saving unlock progression in SQLite
//
// Notes:
// - Persistence model only
// - Unlock requirements and validation belong elsewhere
// ============================================================

using System;

public sealed class UnlockEntity
{
    public Guid UnlockId { get; set; }
    public Guid PlayerId { get; set; }
    public string UnlockDefinitionKey { get; set; } = string.Empty;
    public string UnlockType { get; set; } = string.Empty;
    public DateTime UnlockedUtc { get; set; }

    public PlayerEntity Player { get; set; }
}