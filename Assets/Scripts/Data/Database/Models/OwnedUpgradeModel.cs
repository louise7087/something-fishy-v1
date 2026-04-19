using System;

public class OwnedUpgradeModel
{
    public Guid UpgradeOwnershipId { get; set; }
    public string UpgradeDefinitionKey { get; set; }
    public int Level { get; set; }
}