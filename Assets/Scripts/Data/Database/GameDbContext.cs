using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

public class GameDbContext : DbContext
{
    private readonly String _databasePath;

    public GameDbContext(String databasePath)
    {
        _databasePath = databasePath;
    }

    public DbSet<PlayerEntity> Players => Set<PlayerEntity>();
    public DbSet<WalletEntity> Wallets => Set<WalletEntity>();
    public DbSet<InventoryItemEntity> InventoryItems => Set<InventoryItemEntity>();
    public DbSet<MarketOrderResultEntity> MarketOrderResults => Set<MarketOrderResultEntity>();
    public DbSet<MarketPriceSnapshotEntity> MarketPriceSnapshots => Set<MarketPriceSnapshotEntity>();
    public DbSet<MarketSyncStateEntity> MarketSyncStates => Set<MarketSyncStateEntity>();
    public DbSet<TransactionEntity> Transactions => Set<TransactionEntity>();
    public DbSet<UnlockEntity> Unlocks => Set<UnlockEntity>();
    public DbSet<UpgradeOwnershipEntity> OwnedUpgrades => Set<UpgradeOwnershipEntity>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite($"Data Source={_databasePath}");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PlayerEntity>()
            .HasKey(p => p.PlayerId);

        modelBuilder.Entity<WalletEntity>()
            .HasKey(w => w.WalletId);

        modelBuilder.Entity<InventoryItemEntity>()
            .HasKey(i => i.InventoryItemId);

        modelBuilder.Entity<MarketOrderResultEntity>()
            .HasKey(m => m.OrderResultId);

        modelBuilder.Entity<MarketPriceSnapshotEntity>()
            .HasKey(m => m.MarketPriceSnapshotId);

        modelBuilder.Entity<MarketSyncStateEntity>()
            .HasKey(m => m.MarketSyncStateId);

        modelBuilder.Entity<TransactionEntity>()
            .HasKey(t => t.TransactionId);

        modelBuilder.Entity<UnlockEntity>()
            .HasKey(u => u.UnlockId);

        modelBuilder.Entity<UpgradeOwnershipEntity>()
            .HasKey(u => u.UpgradeOwnershipId);

        modelBuilder.Entity<PlayerEntity>()
            .HasOne(p => p.Wallet)
            .WithOne(w => w.Player)
            .HasForeignKey<WalletEntity>(w => w.PlayerId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<PlayerEntity>()
            .HasMany(p => p.InventoryItems)
            .WithOne(i => i.Player)
            .HasForeignKey(i => i.PlayerId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<PlayerEntity>()
            .HasMany(p => p.UpgradesOwnerships)
            .WithOne(u => u.Player)
            .HasForeignKey(u => u.PlayerId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<PlayerEntity>()
            .HasMany(p => p.Unlocks)
            .WithOne(u => u.Player)
            .HasForeignKey(u => u.PlayerId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<TransactionEntity>()
            .HasIndex(t => new {t.PlayerId, t.IdempotencyKey})
            .IsUnique();

        modelBuilder.Entity<InventoryItemEntity>()
            .HasIndex(i => new { i.PlayerId, i.ItemDefinitionKey, i.SlotIndex })
            .IsUnique();

        modelBuilder.Entity<UpgradeOwnershipEntity>()
            .HasIndex(u => new { u.PlayerId, u.UpgradeDefinitionKey })
            .IsUnique();

        modelBuilder.Entity<UnlockEntity>()
            .HasIndex(u => new { u.PlayerId, u.UnlockDefinitionKey })
            .IsUnique();

        modelBuilder.Entity<MarketPriceSnapshotEntity>()
            .HasIndex(m => m.FishDefinitionKey)
            .IsUnique();
    }
}


