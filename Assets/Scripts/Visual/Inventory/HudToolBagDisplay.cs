using System.Collections.Generic;
using UnityEngine;

public class HudToolBagDisplay : DataDrivenUI<ToolBag>
{
    [SerializeField] private HudToolBagItemDisplay toolBagItemDisplayPrefab;
    [SerializeField] private Transform toolBagItemDisplayContainer;

    private readonly List<HudToolBagItemDisplay> toolBagItemDisplayInstances;

    private HudToolBagDisplay()
        :base()
    {
        toolBagItemDisplayInstances = new List<HudToolBagItemDisplay>();
    }

    protected override void OnValueChanged_Data(ToolBag oldValue, ToolBag newValue)
    {
        if (oldValue != null)
        {
            foreach (HudToolBagItemDisplay instance in toolBagItemDisplayInstances)
                GameObject.Destroy(instance.gameObject);

            toolBagItemDisplayInstances.Clear();

            oldValue.toolBagItemCategoriesFilled.onAdd -= OnAdd_ToolBagItemCategory;
            oldValue.toolBagItemCategoriesFilled.onRemove -= OnRemove_ToolBagItemCategory;
        }

        if (newValue != null)
        {
            newValue.toolBagItemCategoriesFilled.onAdd += OnAdd_ToolBagItemCategory;
            newValue.toolBagItemCategoriesFilled.onRemove += OnRemove_ToolBagItemCategory;
            foreach (ToolBagItemCategory instance in newValue.toolBagItemCategoriesFilled)
                OnAdd_ToolBagItemCategory(instance);
        }
    }

    private void OnAdd_ToolBagItemCategory(ToolBagItemCategory item)
    {
        HudToolBagItemDisplay instance = GameObject.Instantiate(toolBagItemDisplayPrefab, toolBagItemDisplayContainer);
        instance.data = item;
        toolBagItemDisplayInstances.Add(instance);
    }

    private void OnRemove_ToolBagItemCategory(ToolBagItemCategory item)
    {
        HudToolBagItemDisplay instance = toolBagItemDisplayInstances.Find(i => i.data == item);
        GameObject.Destroy(instance.gameObject);
        toolBagItemDisplayInstances.Remove(instance);
    }
}