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

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.VisualScripting;

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

    public async Task SeedDefaultsAsync()
    {
        await using var db = new GameDbContext(_databasePath);

        var defaults = new List<MarketPriceSnapshotEntity>
        {
            new MarketPriceSnapshotEntity { FishDefinitionKey = "fish.parrot", CurrentBuyPriceMinorUnits = 20, CurrentSellPriceMinorUnits = 20, PeakSeason = Season.Spring, Difficulty = 3 },
            new MarketPriceSnapshotEntity { FishDefinitionKey = "fish.mackerel", CurrentBuyPriceMinorUnits = 5, CurrentSellPriceMinorUnits = 5, PeakSeason = Season.Spring, Difficulty = 1 },
            new MarketPriceSnapshotEntity { FishDefinitionKey = "fish.clownfish", CurrentBuyPriceMinorUnits = 8, CurrentSellPriceMinorUnits = 8, PeakSeason = Season.Spring, Difficulty = 1 },
            new MarketPriceSnapshotEntity { FishDefinitionKey = "fish.plaice", CurrentBuyPriceMinorUnits = 10, CurrentSellPriceMinorUnits = 10, PeakSeason = Season.Spring, Difficulty = 2 },
            new MarketPriceSnapshotEntity { FishDefinitionKey = "fish.silvereel", CurrentBuyPriceMinorUnits = 15, CurrentSellPriceMinorUnits = 15, PeakSeason = Season.Spring, Difficulty = 2 },
            new MarketPriceSnapshotEntity { FishDefinitionKey = "fish.seahorse", CurrentBuyPriceMinorUnits = 12, CurrentSellPriceMinorUnits = 12, PeakSeason = Season.Spring, Difficulty = 2 },
            new MarketPriceSnapshotEntity { FishDefinitionKey = "fish.lionfish", CurrentBuyPriceMinorUnits = 25, CurrentSellPriceMinorUnits = 25, PeakSeason = Season.Spring, Difficulty = 4 },
            new MarketPriceSnapshotEntity { FishDefinitionKey = "fish.cowfish", CurrentBuyPriceMinorUnits = 10, CurrentSellPriceMinorUnits = 10, PeakSeason = Season.Spring, Difficulty = 2 },

            new MarketPriceSnapshotEntity { FishDefinitionKey = "fish.tuna", CurrentBuyPriceMinorUnits = 15, CurrentSellPriceMinorUnits = 15, PeakSeason = Season.Summer, Difficulty = 3 },
            new MarketPriceSnapshotEntity { FishDefinitionKey = "fish.bandedbutterflyfish", CurrentBuyPriceMinorUnits = 13, CurrentSellPriceMinorUnits = 13, PeakSeason = Season.Summer, Difficulty = 2 },
            new MarketPriceSnapshotEntity { FishDefinitionKey = "fish.atlanticbass", CurrentBuyPriceMinorUnits = 25, CurrentSellPriceMinorUnits = 25, PeakSeason = Season.Summer, Difficulty = 4 },
            new MarketPriceSnapshotEntity { FishDefinitionKey = "fish.bluetang", CurrentBuyPriceMinorUnits = 5, CurrentSellPriceMinorUnits = 5, PeakSeason = Season.Summer, Difficulty = 1 },
            new MarketPriceSnapshotEntity { FishDefinitionKey = "fish.pollock", CurrentBuyPriceMinorUnits = 8, CurrentSellPriceMinorUnits = 8, PeakSeason = Season.Summer, Difficulty = 1 },
            new MarketPriceSnapshotEntity { FishDefinitionKey = "fish.ballanwrasse", CurrentBuyPriceMinorUnits = 15, CurrentSellPriceMinorUnits = 15, PeakSeason = Season.Summer, Difficulty = 3 },
            new MarketPriceSnapshotEntity { FishDefinitionKey = "fish.weaver", CurrentBuyPriceMinorUnits = 5, CurrentSellPriceMinorUnits = 5, PeakSeason = Season.Summer, Difficulty = 1 },
            new MarketPriceSnapshotEntity { FishDefinitionKey = "fish.bream", CurrentBuyPriceMinorUnits = 5, CurrentSellPriceMinorUnits = 5, PeakSeason = Season.Summer, Difficulty = 1 },
            new MarketPriceSnapshotEntity { FishDefinitionKey = "fish.pufferfish", CurrentBuyPriceMinorUnits = 20, CurrentSellPriceMinorUnits = 20, PeakSeason = Season.Summer, Difficulty = 4 },

            new MarketPriceSnapshotEntity { FishDefinitionKey = "fish.cod", CurrentBuyPriceMinorUnits = 10, CurrentSellPriceMinorUnits = 10, PeakSeason = Season.Autumn, Difficulty = 2 },
            new MarketPriceSnapshotEntity { FishDefinitionKey = "fish.dab", CurrentBuyPriceMinorUnits = 8, CurrentSellPriceMinorUnits = 8, PeakSeason = Season.Autumn, Difficulty = 1 },
            new MarketPriceSnapshotEntity { FishDefinitionKey = "fish.flounder", CurrentBuyPriceMinorUnits = 10, CurrentSellPriceMinorUnits = 10, PeakSeason = Season.Autumn, Difficulty = 2 },
            new MarketPriceSnapshotEntity { FishDefinitionKey = "fish.whiting", CurrentBuyPriceMinorUnits = 5, CurrentSellPriceMinorUnits = 5, PeakSeason = Season.Autumn, Difficulty = 1 },
            new MarketPriceSnapshotEntity { FishDefinitionKey = "fish.halibut", CurrentBuyPriceMinorUnits = 20, CurrentSellPriceMinorUnits = 20, PeakSeason = Season.Autumn, Difficulty = 4 },
            new MarketPriceSnapshotEntity { FishDefinitionKey = "fish.herring", CurrentBuyPriceMinorUnits = 8, CurrentSellPriceMinorUnits = 8, PeakSeason = Season.Autumn, Difficulty = 1 },
            new MarketPriceSnapshotEntity { FishDefinitionKey = "fish.stingray", CurrentBuyPriceMinorUnits = 25, CurrentSellPriceMinorUnits = 25, PeakSeason = Season.Autumn, Difficulty = 4 },

            new MarketPriceSnapshotEntity { FishDefinitionKey = "fish.wolfish", CurrentBuyPriceMinorUnits = 20, CurrentSellPriceMinorUnits = 20, PeakSeason = Season.Winter, Difficulty = 3 },
            new MarketPriceSnapshotEntity { FishDefinitionKey = "fish.bonefish", CurrentBuyPriceMinorUnits = 15, CurrentSellPriceMinorUnits = 15, PeakSeason = Season.Winter, Difficulty = 3 },
            new MarketPriceSnapshotEntity { FishDefinitionKey = "fish.cobia", CurrentBuyPriceMinorUnits = 13, CurrentSellPriceMinorUnits = 13, PeakSeason = Season.Winter, Difficulty = 2 },
            new MarketPriceSnapshotEntity { FishDefinitionKey = "fish.blackdrum", CurrentBuyPriceMinorUnits = 18, CurrentSellPriceMinorUnits = 18, PeakSeason = Season.Winter, Difficulty = 3 },
            new MarketPriceSnapshotEntity { FishDefinitionKey = "fish.blobfish", CurrentBuyPriceMinorUnits = 5, CurrentSellPriceMinorUnits = 5, PeakSeason = Season.Winter, Difficulty = 1 },
            new MarketPriceSnapshotEntity { FishDefinitionKey = "fish.pompano", CurrentBuyPriceMinorUnits = 8, CurrentSellPriceMinorUnits = 8, PeakSeason = Season.Winter, Difficulty = 1 },

            new MarketPriceSnapshotEntity { FishDefinitionKey = "fish.sardine", CurrentBuyPriceMinorUnits = 3, CurrentSellPriceMinorUnits = 3, PeakSeason = Season.Spring, Difficulty = 1 },
            new MarketPriceSnapshotEntity { FishDefinitionKey = "fish.angelfish", CurrentBuyPriceMinorUnits = 8, CurrentSellPriceMinorUnits = 8, PeakSeason = Season.Spring, Difficulty = 1 },
            new MarketPriceSnapshotEntity { FishDefinitionKey = "fish.redsnapper", CurrentBuyPriceMinorUnits = 5, CurrentSellPriceMinorUnits = 5, PeakSeason = Season.Spring, Difficulty = 1 },
            new MarketPriceSnapshotEntity { FishDefinitionKey = "fish.salmon", CurrentBuyPriceMinorUnits = 10, CurrentSellPriceMinorUnits = 10, PeakSeason = Season.Spring, Difficulty = 2 },
            new MarketPriceSnapshotEntity { FishDefinitionKey = "fish.anglerfish", CurrentBuyPriceMinorUnits = 30, CurrentSellPriceMinorUnits = 30, PeakSeason = Season.Spring, Difficulty = 4 },
        };



        foreach (var snapshot in defaults)
        {
            bool exists = await db.MarketPriceSnapshots
                .AnyAsync(m => m.FishDefinitionKey == snapshot.FishDefinitionKey);

            if (exists)
            {
                continue;
            }

            snapshot.MarketPriceSnapshotId = Guid.NewGuid();
            snapshot.RetrievedUtc = DateTime.UtcNow;

            db.MarketPriceSnapshots.Add(snapshot);
        }

        await db.SaveChangesAsync();
    }
}