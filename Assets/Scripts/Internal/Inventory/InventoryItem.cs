using SheetCodes;

public class InventoryItem
{
    public readonly ItemRecord itemRecord;
    public readonly EventVariable<InventoryItem, long> itemCount;
    public readonly EventVariable<InventoryItem, long> reservedCount;
    public readonly EventVariable<InventoryItem, bool> matchesFilter;
    public readonly EventVariable<InventoryItem, bool> isLocked;
    protected readonly InventoryCategoryFilter categoryFilter;

    public long availableCount => itemCount.value - reservedCount.value;

    public InventoryItem(ItemRecord itemRecord, long amount, InventoryFilter filter)
    {
        this.itemRecord = itemRecord;
        itemCount = new EventVariable<InventoryItem, long>(this, amount);
        reservedCount = new EventVariable<InventoryItem, long>(this, 0);
        matchesFilter = new EventVariable<InventoryItem, bool>(this, false);
        isLocked = new EventVariable<InventoryItem, bool>(this, false);
        categoryFilter = filter.categoryFilters[itemRecord.ItemCategory.Identifier];
        categoryFilter.selected.onValueChange += OnValueChanged_CategoryFilter_Selected;

        matchesFilter.value = categoryFilter.selected.value;
    }

    private void OnValueChanged_CategoryFilter_Selected(bool oldValue, bool newValue)
    {
        SetMatchesFilter();
    }

    protected virtual void SetMatchesFilter()
    {
        matchesFilter.value = categoryFilter.selected.value;
    }
}