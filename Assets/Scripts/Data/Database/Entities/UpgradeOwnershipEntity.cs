using System;

public sealed class UpgradeOwnershipEntity
{
    public Guid UpgradeOwnershipId { get; set; } = default!;
    public Guid PlayerId { get; set; } = default!;
    public string UpgradeDefinitionKey { get; set; } = string.Empty;
    public int Level { get; set; }
    public DateTime PurchasedUtc { get; set; }

    public PlayerEntity Player { get; set; }
}