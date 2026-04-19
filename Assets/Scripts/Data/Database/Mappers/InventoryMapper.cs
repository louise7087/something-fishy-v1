using System;

public class InventoryMapper
{
    public static InventoryItemModel ToModel(InventoryItemEntity entity)
    {
        return new InventoryItemModel
        {
            InventoryItemId = entity.InventoryItemId,
            ItemDefinitionKey = entity.ItemDefinitionKey,
            Quantity = entity.Quantity,
            SlotIndex = entity.SlotIndex
        };
    }

    public static InventoryItemEntity ToEntity(InventoryItemModel model, Guid playerId)
    {
        return new InventoryItemEntity
        {
            InventoryItemId = model.InventoryItemId,
            PlayerId = playerId,
            ItemDefinitionKey = model.ItemDefinitionKey,
            Quantity = model.Quantity,
            SlotIndex = model.SlotIndex,
            CreatedUtc = DateTime.UtcNow,
            UpdatedUtc = DateTime.UtcNow
        };
    }
}
