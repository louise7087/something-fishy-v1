using System;
using System.Collections.Generic;

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
    public List<UpgradeOwnershipEntity> UpgradesOwnerships { get; set; } = new();
    public List<UnlockEntity> Unlocks { get; set; } = new();
    public List<TransactionEntity> Transactions { get; set; } = new();
}