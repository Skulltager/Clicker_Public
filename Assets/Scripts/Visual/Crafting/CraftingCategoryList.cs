using SheetCodes;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CraftingCategoryList : MonoBehaviour
{
    [SerializeField] private CraftingCategoryItem craftingCategoryItemPrefab;
    [SerializeField] private RectTransform craftingCategoryItemsContainer;

    private readonly List<CraftingCategoryItem> craftingCategoryItemInstances;

    private CraftingCategoryList()
    {
        craftingCategoryItemInstances = new List<CraftingCategoryItem>();
    }

    private void Awake()
    {
        CraftingCategoryIdentifier[] identifiers = Enum.GetValues(typeof(CraftingCategoryIdentifier)) as CraftingCategoryIdentifier[];
        foreach(CraftingCategoryIdentifier identifier in identifiers)
        {
            if (identifier == CraftingCategoryIdentifier.None)
                continue;

            CraftingCategoryItem instance = GameObject.Instantiate(craftingCategoryItemPrefab, craftingCategoryItemsContainer);
            instance.data = identifier.GetRecord();

            craftingCategoryItemInstances.Add(instance);
        }

        CraftingCategoryItem.currentSelected.value = craftingCategoryItemInstances[0];
    }
}