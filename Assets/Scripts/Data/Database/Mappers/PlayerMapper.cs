using System;
using System.Linq;

public class PlayerMapper
{
    public static PlayerSaveModel ToModel(PlayerEntity entity)
    {

        return new PlayerSaveModel
        {
            PlayerId = entity.PlayerId,
            DisplayName = entity.DisplayName,
            ProgressionLevel = entity.ProgressionLevel,
            CurrentAreaKey = entity.CurrentAreaKey,
            EquippedToolKey = entity.EquippedToolKey,
            BalanceMinorUnits = entity.Wallet?.BalanceMinorUnits ?? 0,
            InventoryItems = entity.InventoryItems.Select(u => InventoryMapper.ToModel(u)).ToList(),
            Unlocks = entity.Unlocks.Select(u => UnlockMapper.ToModel(u)).ToList(),
            OwnedUpgrades = entity.UpgradesOwnerships.Select(u => UpgradeMapper.ToModel(u)).ToList()
        };
    }
}