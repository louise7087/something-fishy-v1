using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

public class SavePlayerProgressService
{
    private readonly PlayerRepository _playerRepository;
    private readonly InventoryRepository _inventoryRepository;
    private readonly WalletRepository _walletRepository;

    public SavePlayerProgressService(PlayerRepository playerRepository, InventoryRepository inventoryRepository, WalletRepository walletRepository)
    {
        _playerRepository = playerRepository;
        _inventoryRepository = inventoryRepository;
        _walletRepository = walletRepository;
    }
    public async Task SavePlayerProgressAsync(PlayerSaveModel playerSave)
    {
        var existingPlayer = await _playerRepository.GetByIdAsync(playerSave.PlayerId);
        var now = DateTime.UtcNow;

        if (existingPlayer == null)
        {
            var player = new PlayerEntity
            {
                PlayerId = playerSave.PlayerId,
                DisplayName = playerSave.DisplayName,
                ProgressionLevel = playerSave.ProgressionLevel,
                CurrentAreaKey = playerSave.CurrentAreaKey,
                EquippedToolKey = playerSave.EquippedToolKey,
                CreatedUtc = now,
                UpdatedUtc = now,
            };

            await _playerRepository.AddAsync(player);

            await _walletRepository.AddAsync(new WalletEntity
            {
                WalletId = Guid.NewGuid(),
                PlayerId = playerSave.PlayerId,
                BalanceMinorUnits = playerSave.BalanceMinorUnits,
                UpdatedUtc = now
            });
        }

        else 
        {
            existingPlayer.PlayerId = playerSave.PlayerId;
            existingPlayer.DisplayName = playerSave.DisplayName;
            existingPlayer.ProgressionLevel = playerSave.ProgressionLevel;
            existingPlayer.CurrentAreaKey = playerSave.CurrentAreaKey;
            existingPlayer.EquippedToolKey = playerSave.EquippedToolKey;
            existingPlayer.UpdatedUtc = now;

            await _playerRepository.UpdatePlayerAsync(existingPlayer);

            var wallet = await _walletRepository.GetByPlayerIdAsync(playerSave.PlayerId);

            if (wallet == null)
            {
                wallet = new WalletEntity
                {
                    WalletId = Guid.NewGuid(),
                    PlayerId = playerSave.PlayerId,
                    BalanceMinorUnits = playerSave.BalanceMinorUnits,
                    UpdatedUtc = now
                };

                await _walletRepository.AddAsync(wallet);
            }
            else
            {
                wallet.BalanceMinorUnits = playerSave.BalanceMinorUnits;
                wallet.UpdatedUtc = now;

                await _walletRepository.UpdateAsync(wallet);
            }
        }

        var existingInventoryItems = await _inventoryRepository.GetByPlayerIdAsync(playerSave.PlayerId);

        foreach (var item in existingInventoryItems) 
        {
            await _inventoryRepository.DeleteAsync(item.InventoryItemId);
        }

        foreach (var item in playerSave.InventoryItems) 
        {
            var inventoryItem = new InventoryItemEntity
            {
                InventoryItemId = item.InventoryItemId == Guid.Empty
                    ? Guid.NewGuid()
                    : item.InventoryItemId,
                PlayerId = playerSave.PlayerId,
                ItemDefinitionKey = item.ItemDefinitionKey,
                Quantity = item.Quantity,
                SlotIndex = item.SlotIndex,
                CreatedUtc = now,
                UpdatedUtc = now
            };

            await _inventoryRepository.AddAsync(inventoryItem);
        }
    }
}