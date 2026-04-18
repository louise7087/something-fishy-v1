// ============================================================
// File: TransactionEntity.cs
// Purpose:
// Represents the database row/entity used to persist a single
// economy or market transaction record.
//
// Responsibilities:
// - Store transaction history in SQLite
// - Represent persisted credits, debits, sales, or purchases
// - Support auditability and history tracking
//
// Notes:
// - Persistence model only
// - Keep transactional rule validation outside this entity
// ============================================================

using System;

public sealed class TransactionEntity
{
    public Guid TransactionId { get; set; }
    public Guid PlayerId { get; set; }
    public string TransactionType { get; set; } = string.Empty; // SellFish, BuyUpgrade, BuySlot
    public int AmountMinorUnits { get; set; }              // positive/negative convention
    public string CurrencyCode { get; set; } = "COIN";
    public string ReferenceId { get; set; } = string.Empty;    // fish id / upgrade id / order id
    public string IdempotencyKey { get; set; } = string.Empty;
    public DateTime CreatedUtc { get; set; }

    public PlayerEntity Player { get; set; } = null!;
}