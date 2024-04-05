using SheetCodes;
using System;
using System.Linq;

public class ToolBag
{
    public readonly int index;
    public readonly EventVariable<ToolBag, bool> selected;
    public readonly EventList<QualityInventoryItem> toolBagItems;
    public readonly EventList<ToolBagItemCategory> toolBagItemCategories;
    public readonly EventList<ToolBagItemCategory> toolBagItemCategoriesFilled;
    public readonly EventVariable<ToolBag, ToolBagItemCategory> equippedItemCategory;

    public ToolBag(int index)
    {
        this.index = index;
        selected = new EventVariable<ToolBag, bool>(this, false);
        toolBagItems = new EventList<QualityInventoryItem>();
        equippedItemCategory = new EventVariable<ToolBag, ToolBagItemCategory>(this, null);
        toolBagItemCategories = new EventList<ToolBagItemCategory>();
        toolBagItemCategoriesFilled = new EventList<ToolBagItemCategory>();
        toolBagItems.onAdd += OnAdd_ToolBagItems;
        toolBagItems.onRemove += OnRemove_ToolBagItems;

        ItemCategoryIdentifier[] itemCategories = Enum.GetValues(typeof(ItemCategoryIdentifier)) as ItemCategoryIdentifier[];
        ItemCategoryRecord[] records = itemCategories
            .Where(i => i != ItemCategoryIdentifier.None)
            .Select(i => i.GetRecord())
            .Where(i => i.CanEquip).ToArray();

        foreach (ItemCategoryRecord record in records)
            toolBagItemCategories.Add(new ToolBagItemCategory(this, record));
    }

    public bool TryEquipItemCategory(ItemCategoryIdentifier itemCategory)
    {
        ToolBagItemCategory toolBagItemCategory = toolBagItemCategoriesFilled.Find(i => i.itemCategoryRecord.Identifier == itemCategory);
        if (toolBagItemCategory == null)
            return false;

        equippedItemCategory.value = toolBagItemCategory;
        if (!toolBagItemCategory.TryEquipItem())
            return false;

        return true;
    }

    private void OnAdd_ToolBagItems(QualityInventoryItem item)
    {
        ToolBagItemCategory toolBagItems = toolBagItemCategories.Find(i => i.itemCategoryRecord == item.itemRecord.ItemCategory);
        toolBagItems.inventoryItems.Add(item);
    }

    private void OnRemove_ToolBagItems(QualityInventoryItem item)
    {
        ToolBagItemCategory toolBagItems = toolBagItemCategories.Find(i => i.itemCategoryRecord == item.itemRecord.ItemCategory);
        toolBagItems.inventoryItems.Remove(item);
    }
}