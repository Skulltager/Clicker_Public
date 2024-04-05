using SheetCodes;

public class ToolStats
{
    public readonly ToolBaseStatsRecord statsRecord;

    public readonly ValueTracker_Float minCraftingQuality;
    public readonly ValueTracker_Float maxCraftingQuality;
    public readonly ValueTracker_Int durability;
    public readonly ValueTracker_Int damage;
    public readonly ValueTracker_Float itemDropRate;
    public readonly ValueTracker_Float tokenDropRate;

    public ToolStats(ToolBaseStatsRecord statsRecord)
    {
        this.statsRecord = statsRecord;
        minCraftingQuality = new ValueTracker_Float(50);
        maxCraftingQuality = new ValueTracker_Float(100);
        durability = new ValueTracker_Int(statsRecord.Durability);
        damage = new ValueTracker_Int(statsRecord.Damage);
        itemDropRate = new ValueTracker_Float(statsRecord.ItemDropRateMultiplier);
        tokenDropRate = new ValueTracker_Float(statsRecord.TokenDropRateMultiplier);
    }
}
