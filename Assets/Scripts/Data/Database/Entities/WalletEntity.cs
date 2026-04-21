using System;

public sealed class WalletEntity
{
    public Guid WalletId { get; set; }
    public Guid PlayerId { get; set; }
    public int BalanceMinorUnits { get; set; }
    public DateTime UpdatedUtc { get; set; }

    public PlayerEntity Player { get; set; } = null!;
}