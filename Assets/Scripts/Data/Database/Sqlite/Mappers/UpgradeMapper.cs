public class UpgradeMapper
{
    public OwnedUpgradeModel ToModel(UpgradeOwnershipEntity entity)
    {
        return new OwnedUpgradeModel
        {
            UpgradeOwnershipId = entity.UpgradeOwnershipId,
            UpgradeDefinitionKey = entity.UpgradeDefinitionKey,
            Level = entity.Level
        };
    }
}