// ============================================================
// File: MarketPriceRepository.cs
// Purpose:
// Handles reading and writing market price data to
// the SQLite database.
//
// Responsibilities:
// - Load market price history/snapshot data
// - Save market price history/snapshot data
// - Isolate market activity persistence from gameplay services
//
// Notes:
// - Infrastructure only
// - Keep pricing and simulation logic outside this repository
// ============================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

public class MarketPriceRepository
{
    private readonly string _databasePath;
    public MarketPriceRepository(string databasePath)
    {
        _databasePath = databasePath;
    }
    public async Task<List<MarketPriceSnapshotEntity>> GetAllAsync()
    {
        await using var db = new GameDbContext(_databasePath);
        return await db.MarketPriceSnapshots
            .OrderBy(x => x.FishDefinitionKey)
            .ToListAsync();
    }

    public async Task<MarketPriceSnapshotEntity> GetByFishDefinitionKey(string fishDefinitionKey)
    {
        await using var db = new GameDbContext(_databasePath);
        return await db.MarketPriceSnapshots
            .FirstOrDefaultAsync(m => m.FishDefinitionKey == fishDefinitionKey);
    }

    public async Task AddAsync(MarketPriceSnapshotEntity marketPriceSnapshot)
    {
        await using var db = new GameDbContext(_databasePath);
        await db.MarketPriceSnapshots.AddAsync(marketPriceSnapshot);
        await db.SaveChangesAsync();
    }

    public async Task UpdateAsync(MarketPriceSnapshotEntity marketPriceSnapshot)
    {
        await using var db = new GameDbContext(_databasePath);
        db.MarketPriceSnapshots.Update(marketPriceSnapshot);
        await db.SaveChangesAsync();
    }
}