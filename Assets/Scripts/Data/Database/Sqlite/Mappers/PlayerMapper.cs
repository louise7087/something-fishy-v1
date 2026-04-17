using System;
using System.Linq;

public class PlayerMapper
{
    public PlayerSaveModel ToModel(PlayerEntity entity)
    {
        var unlockMapper = new UnlockMapper();
        var upgradeMapper = new UpgradeMapper();
        var inventoryMapper = new InventoryMapper();

        return new PlayerSaveModel
        {
            PlayerId = entity.PlayerId,
            DisplayName = entity.DisplayName,
            ProgressionLevel = entity.ProgressionLevel,
            CurrentAreaKey = entity.CurrentAreaKey,
            EquippedToolKey = entity.EquippedToolKey,
            BalanceMinorUnits = entity.Wallet?.BalanceMinorUnits ?? 0,
            InventoryItems = entity.InventoryItems.Select(u => inventoryMapper.ToModel(u)).ToList(),
            Unlocks = entity.Unlocks.Select(u => unlockMapper.ToModel(u)).ToList(),
            OwnedUpgrades = entity.UpgradesOwnership.Select(u => upgradeMapper.ToModel(u)).ToList()
        };
    }
}