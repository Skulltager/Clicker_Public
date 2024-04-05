using SheetCodes;
using System.Collections.Generic;
using UnityEngine;

public class CraftingRecipeList : MonoBehaviour
{
    [SerializeField] private CraftingRecipeItem craftingRecipeItemPrefab;
    [SerializeField] private RectTransform craftingRecipeItemsContainer;

    private readonly List<CraftingRecipeItem> craftingRecipeItemInstances;

    private CraftingRecipeList()
    {
        craftingRecipeItemInstances = new List<CraftingRecipeItem>();
    }

    private void Awake()
    {
        CraftingCategoryItem.currentSelected.onValueChangeImmediate += OnValueChanged_SelectedCraftingCategory;
    }

    private void OnValueChanged_SelectedCraftingCategory(CraftingCategoryItem oldValue, CraftingCategoryItem newValue)
    {
        if (oldValue != null)
        {
            foreach (CraftingRecipeItem instance in craftingRecipeItemInstances)
                GameObject.Destroy(instance.gameObject);

            craftingRecipeItemInstances.Clear();
        }

        if(newValue != null)
        {
            List<CraftingRecipeRecord> craftingRecipes = GameData.instance.GetCategoryRecipes(newValue.data.Identifier);
            foreach(CraftingRecipeRecord record in craftingRecipes)
            {
                CraftingRecipeItem instance = GameObject.Instantiate(craftingRecipeItemPrefab, craftingRecipeItemsContainer);
                instance.data = record;
                craftingRecipeItemInstances.Add(instance);
            }
        }
    }
    
    private void OnDestroy()
    {
        CraftingCategoryItem.currentSelected.onValueChangeImmediate -= OnValueChanged_SelectedCraftingCategory;
    }
}