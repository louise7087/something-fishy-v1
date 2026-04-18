// ============================================================
// File: PlayerRepository.cs
// Purpose:
// Handles reading and writing player-related persisted data
// to the SQLite database.
//
// Responsibilities:
// - Load player persistence data
// - Save player persistence data
// - Isolate player persistence concerns from gameplay logic
//
// Notes:
// - Infrastructure only
// - Keep storage schema concerns out of runtime controllers
// ============================================================

using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

public class PlayerRepository
{
    private readonly string _databasePath;

    public PlayerRepository(string databasePath)
    {
        _databasePath = databasePath;
    }

    public async Task<PlayerEntity?> GetByIdAsync(Guid playerId)
    {
        await using var db = new GameDbContext(_databasePath);

        return await db.Players
            .Include(p => p.Wallet)
            .Include(p => p.InventoryItems)
            .Include(p => p.UpgradesOwnerships)
            .Include(p => p.Unlocks)
            .Include(p => p.Transactions)
            .FirstOrDefaultAsync(p => p.PlayerId == playerId);
    }

    public async Task AddAsync(PlayerEntity player) 
    { 
        await using var db = new GameDbContext(_databasePath);
        await db.Players.AddAsync(player);
        await db.SaveChangesAsync();
    }

    public async Task UpdateAsync(PlayerEntity player)
    {
        await using var db = new GameDbContext(_databasePath);
        db.Players.Update(player);
        await db.SaveChangesAsync();
    }
}