using SheetCodes;

public class InventoryQualityFilter
{
    public readonly EventVariable<InventoryQualityFilter, bool> selected;
    public readonly ItemQualityRecord record;

    public InventoryQualityFilter(ItemQualityRecord record, bool selected = true)
    {
        this.record = record;
        this.selected = new EventVariable<InventoryQualityFilter, bool>(this, selected);
    }
}
