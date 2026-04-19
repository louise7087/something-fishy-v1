public class UnlockMapper
{
    public static UnlockModel ToModel(UnlockEntity entity)
    {
        return new UnlockModel
        {
            UnlockId = entity.UnlockId,
            UnlockDefinitionKey = entity.UnlockDefinitionKey,
            UnlockType = entity.UnlockType
        };
    }
}