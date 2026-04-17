using System;
using System.Collections.Generic;

public class InventoryItemModel
{
    public Guid InventoryItemId { get; set; }
    public string ItemDefinitionId { get; set; }
    public int Quantity { get; set; }
    public int SlotIndex { get; set; }
}