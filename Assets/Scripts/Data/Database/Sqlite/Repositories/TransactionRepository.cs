// ============================================================
// File: TransactionRepository.cs
// Purpose:
// Handles reading and writing transaction history data to the
// SQLite database.
//
// Responsibilities:
// - Load transaction records
// - Save transaction records
// - Support audit/history persistence for economy operations
//
// Notes:
// - Infrastructure only
// - Keep economy rules and validation outside this repository
// ============================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

public class TransactionRepository
{
    private readonly string _databasePath;
    public TransactionRepository(string databasePath)
    {
        _databasePath = databasePath;
    }
    public async Task<List<TransactionEntity>> GetByPlayerIdAsync(Guid playerId)
    {
        await using var db = new GameDbContext(_databasePath);
        
        return await db.Transactions
            .Where (t => t.PlayerId == playerId)
            .OrderByDescending (t => t.CreatedUtc)
            .ToListAsync();
    }
    public async Task AddAsync(TransactionEntity transaction) 
    { 
        await using var db = new GameDbContext(_databasePath);
        await db.Transactions.AddAsync(transaction);
        await db.SaveChangesAsync();
    }
}