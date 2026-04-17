using System;
using System.Collections.Generic;

public class PlayerSaveModel
{
    public Guid PlayerId { get; set; }
    public string DisplayName { get; set; } = string.Empty;
    public int ProgressionLevel { get; set; }
    public string CurrentAreaKey { get; set; } = "starter_area";
    public string EquippedToolKey { get; set; } = "basic_rod";
    public int BalanceMinorUnits { get; set; }
    public List<InventoryItemModel> InventoryItems { get; set; } = new();
    public List<OwnedUpgradeModel> OwnedUpgrades { get; set; } = new();
    public List<UnlockModel> Unlocks { get; set; } = new();
}