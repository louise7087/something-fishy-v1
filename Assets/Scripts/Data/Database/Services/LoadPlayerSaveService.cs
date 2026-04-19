using System;
using System.Threading.Tasks;
using UnityEngine;

public class LoadPlayerService
{
    private const string PLAYER_ID_KEY = "localPlayerId";

    private readonly PlayerRepository _playerRepository;

    public LoadPlayerService(PlayerRepository playerRepository)
    {
        _playerRepository = playerRepository;
    }

    public PlayerEntity CreateDefaultPlayer(Guid playerId, string displayName = "Player")
    {
        var now = DateTime.UtcNow;

        return new PlayerEntity
        {
            PlayerId = playerId,
            DisplayName = displayName,
            ProgressionLevel = 0,
            CurrentAreaKey = "starter_area",
            EquippedToolKey = "basic_rod",
            CreatedUtc = now,
            UpdatedUtc = now,
            Wallet = new WalletEntity
            {
                WalletId = Guid.NewGuid(),
                PlayerId = playerId,
                BalanceMinorUnits = 0,
                UpdatedUtc = now,
            }
        };
    }

    public async Task<LoadPlayerSaveResult> LoadorCreateLocalPlayerAsync()
    {
        Guid playerId;

        if (PlayerPrefs.HasKey(PLAYER_ID_KEY))
        {
            playerId = Guid.Parse(PlayerPrefs.GetString(PLAYER_ID_KEY));
        }
        else
        {
            playerId = Guid.NewGuid();
            PlayerPrefs.SetString(PLAYER_ID_KEY, playerId.ToString());
            PlayerPrefs.Save();
        }

        return await LoadOrCreatePlayerAsync(playerId);
    }

    public async Task<LoadPlayerSaveResult> LoadOrCreatePlayerAsync(Guid playerId)
    {
        Guid guid = Guid.NewGuid();
        var playerEntity = await _playerRepository.GetByIdAsync(playerId);
        if (playerEntity != null)
        {
            ActivePlayerSession.CurrentPlayerId = playerEntity.PlayerId;
            return new LoadPlayerSaveResult
            {
                WasNewPlayerCreated = false,
                Player = PlayerMapper.ToModel(playerEntity),
            };
        }
        else
        {
            var newPlayerEntity = CreateDefaultPlayer(playerId);
            await _playerRepository.AddAsync(newPlayerEntity);
            ActivePlayerSession.CurrentPlayerId = newPlayerEntity.PlayerId;

            return new LoadPlayerSaveResult
            {
                WasNewPlayerCreated = true,
                Player = PlayerMapper.ToModel(newPlayerEntity),
            };
        }
    }
}