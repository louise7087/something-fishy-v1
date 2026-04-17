public class UnlockMapper
{
    public UnlockModel ToModel(UnlockEntity entity)
    {
        return new UnlockModel
        {
            UnlockId = entity.UnlockId,
            UnlockDefinitionKey = entity.UnlockDefinitionKey,
            UnlockType = entity.UnlockType
        };
    }
}