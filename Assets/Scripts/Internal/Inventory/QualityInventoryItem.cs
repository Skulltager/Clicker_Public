using SheetCodes;

public class QualityInventoryItem : InventoryItem
{
    public readonly ItemQualityRecord qualityRecord;
    public readonly ToolBaseStatsRecord statsRecord;
    private readonly InventoryQualityFilter qualityFilter;
    public readonly ControlledEventVariable<QualityInventoryItem, int> durabilityLeft;
    public readonly EventVariable<QualityInventoryItem, bool> toolBagEquipped;

    private int Check_DurabilityLeft(int value)
    {
        if (value > 0)
            return value;

        int amountUsed = -value / statsRecord.Durability + 1;
        itemCount.value -= amountUsed;
        value += amountUsed * statsRecord.Durability;
        return value;
    }

    public QualityInventoryItem(ItemRecord itemRecord, long amount, InventoryFilter filter, ItemQualityRecord qualityRecord) 
        : base(itemRecord, amount, filter)
    {
        this.qualityRecord = qualityRecord;
        qualityFilter = filter.qualityFilters[qualityRecord.Identifier];
        qualityFilter.selected.onValueChangeImmediate += OnValueChanged_QualityFilterSelected;
        statsRecord = ModelManager.ToolBaseStatsModel.GetMatchingRecord(qualityRecord, itemRecord);
        durabilityLeft = new ControlledEventVariable<QualityInventoryItem, int>(this, statsRecord.Durability, Check_DurabilityLeft);
        toolBagEquipped = new EventVariable<QualityInventoryItem, bool>(this, false);
    }

    private void OnValueChanged_QualityFilterSelected(bool oldValue, bool newValue)
    {
        SetMatchesFilter();
    }

    protected override void SetMatchesFilter()
    {
        if (!categoryFilter.selected.value)
        {
            matchesFilter.value = false;
            return;
        }

        if (!qualityFilter.selected.value)
        {
            matchesFilter.value = false;
            return;
        }

        matchesFilter.value = true;
    }

    public void Use()
    {
        durabilityLeft.value--;
    }
}
