using System;
using System.Collections.Generic;

public class PlayerSaveModel
{
    public Guid PlayerId { get; set; }
    public string DisplayName { get; set; } = string.Empty;
    public int ProgressionLevel { get; set; }
    public string CurrentAreaKey { get; set; } = "starter_area";
    public string EquippedToolKey { get; set; } = "basic_rod";
    public DateTime CreatedUtc { get; set; }
    public DateTime UpdatedUtc { get; set; }
    public List<InventoryItemModel> InventoryItems { get; set; } = new();
    public List<OwnedUpgradeModel> UpgradesOwnership { get; set; } = new();
    public List<UnlockModel> Unlocks { get; set; } = new();
}