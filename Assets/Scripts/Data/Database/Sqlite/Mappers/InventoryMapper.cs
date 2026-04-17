using System;

public class InventoryMapper
{
    public InventoryItemModel ToModel(InventoryItemEntity entity)
    {
        return new InventoryItemModel
        {
            InventoryItemId = entity.InventoryItemId,
            ItemDefinitionKey = entity.ItemDefinitionKey,
            Quantity = entity.Quantity,
            SlotIndex = entity.SlotIndex
        };
    }

    public InventoryItemEntity ToEntity(InventoryItemModel model, Guid playerId)
    {
        return new InventoryItemEntity
        {
            InventoryItemId = model.InventoryItemId,
            PlayerId = playerId,
            ItemDefinitionKey = model.ItemDefinitionKey,
            Quantity = model.Quantity,
            SlotIndex = model.SlotIndex,
            CreatedUtcTicks = DateTime.UtcNow,
            UpdatedUtcTicks = DateTime.UtcNow
        };
    }
}
