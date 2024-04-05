using System.Collections.Generic;
using UnityEngine;
using SheetCodes;
using System;

public class InventoryFilterDisplay : DataDrivenUI<InventoryFilter>
{
    [SerializeField] private InventoryCategoryButton categoryButtonPrefab;
    [SerializeField] private InventoryQualityButton qualityButtonPrefab;
    [SerializeField] private RectTransform categoryButtonsContainer;
    [SerializeField] private RectTransform qualityButtonsContainer;

    private readonly List<InventoryCategoryButton> categoryButtonInstances;
    private readonly List<InventoryQualityButton> qualityButtonInstances;

    private InventoryFilterDisplay()
        : base()
    {
        categoryButtonInstances = new List<InventoryCategoryButton>();
        qualityButtonInstances = new List<InventoryQualityButton>();
    }

    protected override void OnValueChanged_Data(InventoryFilter oldValue, InventoryFilter newValue)
    {
        if (oldValue != null)
        {
            foreach (InventoryCategoryButton instance in categoryButtonInstances)
                GameObject.Destroy(instance.gameObject);

            foreach (InventoryQualityButton instance in qualityButtonInstances)
                GameObject.Destroy(instance.gameObject);

            categoryButtonInstances.Clear();
            qualityButtonInstances.Clear();
        }

        if (newValue != null)
        {
            ItemQualityIdentifier[] itemQualityIdentifiers = Enum.GetValues(typeof(ItemQualityIdentifier)) as ItemQualityIdentifier[];
            foreach (ItemQualityIdentifier identifier in itemQualityIdentifiers)
            {
                if (identifier == ItemQualityIdentifier.None)
                    continue;

                InventoryQualityButton instance = GameObject.Instantiate(qualityButtonPrefab, qualityButtonsContainer);
                instance.data = data.qualityFilters[identifier];
                qualityButtonInstances.Add(instance);
            }

            ItemCategoryIdentifier[] itemCategoryIdentifiers = Enum.GetValues(typeof(ItemCategoryIdentifier)) as ItemCategoryIdentifier[];
            foreach (ItemCategoryIdentifier identifier in itemCategoryIdentifiers)
            {
                if (identifier == ItemCategoryIdentifier.None)
                    continue;
                
                InventoryCategoryButton instance = GameObject.Instantiate(categoryButtonPrefab, categoryButtonsContainer);
                instance.data = data.categoryFilters[identifier];
                categoryButtonInstances.Add(instance);
            }
        }
    }
}