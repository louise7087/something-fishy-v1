using System;

public class UnlockModel
{
    public Guid UnlockId { get; set; }
    public string UnlockKey { get; set; } = string.Empty;
    public string UnlockType { get; set; } = string.Empty;
}