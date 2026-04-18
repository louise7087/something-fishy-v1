// ============================================================
// File: UnlockRepository.cs
// Purpose:
// Handles reading and writing progression unlock data to the
// SQLite database.
//
// Responsibilities:
// - Load player/world unlock persistence data
// - Save player/world unlock persistence data
// - Isolate unlock storage concerns from progression logic
//
// Notes:
// - Infrastructure only
// - Unlock conditions and gating logic belong elsewhere
// ============================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

public class UnlockRepository
{
    private readonly string _databasePath;
    public UnlockRepository(string databasePath)
    {
        _databasePath = databasePath;
    }
    public async Task<List<UnlockEntity>> GetByPlayerIdAsync(Guid playerId)
    {
        await using var db = new GameDbContext(_databasePath);
        
        return await db.Unlocks
            .Where (u => u.PlayerId == playerId)
            .ToListAsync();
    }
    public async Task AddAsync(UnlockEntity unlock) 
    { 
        await using var db = new GameDbContext(_databasePath);
        await db.Unlocks.AddAsync(unlock);
        await db.SaveChangesAsync();
    }
}