using SheetCodes;

public class InventoryCategoryFilter
{
    public readonly ItemCategoryRecord record;
    public readonly EventVariable<InventoryCategoryFilter, bool> selected;

    public InventoryCategoryFilter(ItemCategoryRecord record, bool selected = true)
    {
        this.record = record;
        this.selected = new EventVariable<InventoryCategoryFilter, bool>(this, selected);
    }
}
