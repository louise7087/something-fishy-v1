// ============================================================
// File: WalletRepository.cs
// Purpose:
// Handles reading and writing wallet or player currency data
// to the SQLite database.
//
// Responsibilities:
// - Load persisted wallet/currency data
// - Save persisted wallet/currency data
// - Isolate economy balance storage from gameplay logic
//
// Notes:
// - Infrastructure only
// - Debit/credit validation belongs in economy domain/services
// ============================================================

using System;
using System.Threading.Tasks;
using Assets.Scripts.Data.Database.Sqlite;
using Microsoft.EntityFrameworkCore;

public class WalletRepository
{
    private readonly string _databasePath;
    public WalletRepository(string databasePath)
    {
        _databasePath = databasePath;
    }
    public async Task<WalletEntity?> GetByPlayerIdAsync(Guid playerId)
    {
        await using var db = new GameDbContext(_databasePath);
        return await db.Wallets
            .FirstOrDefaultAsync(w => w.PlayerId == playerId);
    }
    public async Task AddAsync(WalletEntity wallet) 
    { 
        await using var db = new GameDbContext(_databasePath);
        await db.Wallets.AddAsync(wallet);
        await db.SaveChangesAsync();
    }
    public async Task UpdateAsync(WalletEntity wallet)
    {
        await using var db = new GameDbContext(_databasePath);
        db.Wallets.Update(wallet);
        await db.SaveChangesAsync();
    }
}