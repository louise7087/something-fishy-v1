using System;
using System.Threading.Tasks;
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