using SheetCodes;
using System.Collections.Generic;
using System;
using System.Linq;

public class InventoryFilter 
{
    public readonly Dictionary<ItemCategoryIdentifier, InventoryCategoryFilter> categoryFilters;
    public readonly Dictionary<ItemQualityIdentifier, InventoryQualityFilter> qualityFilters;

    public InventoryFilter()
    {
        categoryFilters = new Dictionary<ItemCategoryIdentifier, InventoryCategoryFilter>();
        qualityFilters = new Dictionary<ItemQualityIdentifier, InventoryQualityFilter>();

        ItemCategoryIdentifier[] categoryIdentifiers = Enum.GetValues(typeof(ItemCategoryIdentifier)) as ItemCategoryIdentifier[];
        ItemQualityIdentifier[] qualityIdentifiers = Enum.GetValues(typeof(ItemQualityIdentifier)) as ItemQualityIdentifier[];

        ItemCategoryRecord[] categoryRecords = categoryIdentifiers.Where(i => i != ItemCategoryIdentifier.None).Select(i => i.GetRecord()).ToArray();
        ItemQualityRecord[] qualityRecord = qualityIdentifiers.Where(i => i != ItemQualityIdentifier.None).Select(i => i.GetRecord()).ToArray();

        foreach(ItemCategoryRecord record in categoryRecords)
            categoryFilters.Add(record.Identifier, new InventoryCategoryFilter(record));

        foreach (ItemQualityRecord record in qualityRecord)
            qualityFilters.Add(record.Identifier, new InventoryQualityFilter(record));
    }
}