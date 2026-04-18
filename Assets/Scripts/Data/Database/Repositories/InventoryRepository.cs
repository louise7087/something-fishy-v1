// ============================================================
// File: InventoryRepository.cs
// Purpose:
// Handles reading and writing inventory and item-related persisted data
// to the SQLite database.
//
// Responsibilities:
// - Load inventory and item persistence data
// - Save inventory and item persistence data
// - Keep SQL/data-access logic isolated from gameplay code
//
// Notes:
// - Infrastructure only
// - Should depend on DB connection layer, not UI/gameplay
// ============================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

public class InventoryRepository
{
    private readonly string _databasePath;
    public InventoryRepository(string databasePath)
    {
        _databasePath = databasePath;
    }

    public async Task<List<InventoryItemEntity>> GetByPlayerIdAsync(Guid playerId)
    {
        await using var db = new GameDbContext(_databasePath);
        
        return await db.InventoryItems
            .Where (i => i.PlayerId == playerId)
            .OrderBy (i => i.SlotIndex)
            .ToListAsync();
    }

    public async Task<InventoryItemEntity?> GetByIdAsync(Guid inventoryItemId)
    {
        await using var db = new GameDbContext(_databasePath);
        return await db.InventoryItems
            .FirstOrDefaultAsync(i => i.InventoryItemId == inventoryItemId);
    }
    public async Task AddAsync(InventoryItemEntity inventoryItem) 
    { 
        await using var db = new GameDbContext(_databasePath);
        await db.InventoryItems.AddAsync(inventoryItem);
        await db.SaveChangesAsync();
    }
    public async Task UpdateAsync(InventoryItemEntity inventoryItem)
    {
        await using var db = new GameDbContext(_databasePath);
        db.InventoryItems.Update(inventoryItem);
        await db.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id) 
    { 
        await using var db = new GameDbContext(_databasePath);
        var entity = await db.InventoryItems.FirstOrDefaultAsync(i => i.InventoryItemId == id);
        if (entity != null)
        {
            db.InventoryItems.Remove(entity);
            await db.SaveChangesAsync();
        }
        else { return; }
    }
}