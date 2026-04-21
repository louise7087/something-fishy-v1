using System;

public sealed class UnlockEntity
{
    public Guid UnlockId { get; set; }
    public Guid PlayerId { get; set; }
    public string UnlockDefinitionKey { get; set; } = string.Empty;
    public string UnlockType { get; set; } = string.Empty;
    public DateTime UnlockedUtc { get; set; }

    public PlayerEntity Player { get; set; }
}