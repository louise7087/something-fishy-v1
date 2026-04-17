using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;

public sealed class MarketSyncState
{
    public Guid MarketSyncStateId { get; set; }
    public string SyncScope { get; set; } = "global_market";
    public DateTime LastSyncedUtc { get; set; }
    public string? LastSyncVersion { get; set; }
    public DateTime? LastAttemptUtc { get; set; }
}