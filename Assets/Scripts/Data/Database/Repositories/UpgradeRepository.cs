using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

public class UpgradeRepository
{
    private readonly string _databasePath;
    public UpgradeRepository(string databasePath)
    {
        _databasePath = databasePath;
    }
    public async Task<List<UpgradeOwnershipEntity>> GetByPlayerIdAsync(Guid playerId)
    {
        await using var db = new GameDbContext(_databasePath);
        
        return await db.OwnedUpgrades
            .Where (u => u.PlayerId == playerId)
            .ToListAsync();
    }
    public async Task AddAsync(UpgradeOwnershipEntity upgradeOwnership) 
    { 
        await using var db = new GameDbContext(_databasePath);
        await db.OwnedUpgrades.AddAsync(upgradeOwnership);
        await db.SaveChangesAsync();
    }

    public async Task UpdateAsync(UpgradeOwnershipEntity upgradeOwnership)
    {
        await using var db = new GameDbContext(_databasePath);
        db.OwnedUpgrades.Update(upgradeOwnership);
        await db.SaveChangesAsync();
    }
}