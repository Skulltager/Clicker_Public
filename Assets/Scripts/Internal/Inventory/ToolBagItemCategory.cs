using SheetCodes;

public class ToolBagItemCategory
{
    public readonly ItemCategoryRecord itemCategoryRecord;
    public readonly EventList<QualityInventoryItem> inventoryItems;
    public readonly EventVariable<ToolBagItemCategory, QualityInventoryItem> equippedItem;
    public readonly ToolBag toolBag;

    public ToolBagItemCategory(ToolBag toolBag, ItemCategoryRecord itemCategoryRecord)
    {
        this.toolBag = toolBag;
        this.itemCategoryRecord = itemCategoryRecord;
        inventoryItems = new EventList<QualityInventoryItem>();
        equippedItem = new EventVariable<ToolBagItemCategory, QualityInventoryItem>(this, null);
        equippedItem.onValueChange += OnValueChanged_EquippedItem;
        inventoryItems.onAdd += OnAdd_InventoryItem;
        inventoryItems.onRemove += OnRemove_InventoryItem;
    }

    public bool TryEquipItem()
    {
        if (equippedItem.value != null && equippedItem.value.availableCount > 0)
            return true;

        for(int i = 0; i < inventoryItems.Count; i++)
        {
            QualityInventoryItem item = inventoryItems[i];
            if (item.availableCount == 0)
                continue;

            equippedItem.value = item;
            return true;
        }

        return false;
    }

    private void OnValueChanged_EquippedItem(QualityInventoryItem oldValue, QualityInventoryItem newValue)
    {
        if (oldValue != null)
        {
            oldValue.reservedCount.onValueChange -= OnValueChanged_EquippedItem_ReservedCount;
            oldValue.itemCount.onValueChange -= OnValueChanged_EquippedItem_ItemCount;
        }

        if (newValue != null)
        {
            newValue.reservedCount.onValueChange += OnValueChanged_EquippedItem_ReservedCount;
            newValue.itemCount.onValueChange += OnValueChanged_EquippedItem_ItemCount;
        }
    }

    private void OnValueChanged_EquippedItem_ReservedCount(long oldValue, long newValue)
    {
        CheckEquippedItem();
    }

    private void OnValueChanged_EquippedItem_ItemCount(long oldValue, long newValue)
    {
        CheckEquippedItem();
    }

    private void CheckEquippedItem()
    {
        if (equippedItem.value.availableCount > 0)
            return;

        TryEquipItem();
    }

    private void OnAdd_InventoryItem(QualityInventoryItem item)
    {
        if (inventoryItems.Count == 1)
            toolBag.toolBagItemCategoriesFilled.Add(this);

        if (equippedItem.value != null && equippedItem.value.availableCount > 0)
            return;

        if (item.availableCount == 0)
            return;

        equippedItem.value = item;
    }

    private void OnRemove_InventoryItem(QualityInventoryItem item)
    {
        if (equippedItem.value == item)
        {
            bool found = false;

            for (int i = 0; i < inventoryItems.Count; i++)
            {
                QualityInventoryItem checkItem = inventoryItems[i];
                if (checkItem.availableCount == 0)
                    continue;

                equippedItem.value = checkItem;
                found = true;
                break;
            }

            if(!found)
                equippedItem.value = null;
        }

        if (inventoryItems.Count == 0)
            toolBag.toolBagItemCategoriesFilled.Remove(this);
    }
}