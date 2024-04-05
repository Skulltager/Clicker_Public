using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolBagDisplayData
{
    public readonly PlayerInventory playerInventory;
    public readonly ToolBag toolBag;

    public ToolBagDisplayData(PlayerInventory playerInventory, ToolBag toolBag)
    {
        this.playerInventory = playerInventory;
        this.toolBag = toolBag;
    }
}

public class ToolBagDisplay : DataDrivenUI<ToolBagDisplayData>
{
    [SerializeField] private InventoryItemDisplay inventoryItemDisplayPrefab;
    [SerializeField] private QualityInventoryItemDisplay qualityInventoryItemDisplayPrefab;
    [SerializeField] private RectTransform inventoryItemDisplayContainer;

    private readonly List<InventoryItemDisplay> inventoryItemDisplayInstances;
    private readonly List<QualityInventoryItemDisplay> qualityInventoryItemDisplayInstances;

    private ToolBagDisplay()
        :base()
    {
        inventoryItemDisplayInstances = new List<InventoryItemDisplay>();
        qualityInventoryItemDisplayInstances = new List<QualityInventoryItemDisplay>();
    }

    protected override void OnValueChanged_Data(ToolBagDisplayData oldValue, ToolBagDisplayData newValue)
    {
        if (oldValue != null)
        {
            oldValue.toolBag.toolBagItems.onAdd -= OnAdd_ToolBagItem;
            oldValue.toolBag.toolBagItems.onRemove -= OnRemove_ToolBagItem;

            foreach (InventoryItemDisplay instance in inventoryItemDisplayInstances)
                GameObject.Destroy(instance.gameObject);

            foreach (QualityInventoryItemDisplay instance in qualityInventoryItemDisplayInstances)
                GameObject.Destroy(instance.gameObject);

            qualityInventoryItemDisplayInstances.Clear();
            inventoryItemDisplayInstances.Clear();
        }

        if (newValue != null)
        {
            newValue.toolBag.toolBagItems.onAdd += OnAdd_ToolBagItem;
            newValue.toolBag.toolBagItems.onRemove += OnRemove_ToolBagItem;

            foreach (InventoryItem item in newValue.toolBag.toolBagItems)
                OnAdd_ToolBagItem(item);
        }
    }


    private void OnAdd_ToolBagItem(InventoryItem inventoryItem)
    {
        if (inventoryItem is QualityInventoryItem qualityInventoryItem)
        {
            QualityInventoryItemDisplay qualityInstance = GameObject.Instantiate(qualityInventoryItemDisplayPrefab, inventoryItemDisplayContainer);
            qualityInstance.data = new QualityInventoryItemDisplayData(data.playerInventory, qualityInventoryItem);
            qualityInventoryItemDisplayInstances.Add(qualityInstance);
            return;
        }

        InventoryItemDisplay instance = GameObject.Instantiate(inventoryItemDisplayPrefab, inventoryItemDisplayContainer);
        instance.data = new InventoryItemDisplayData(data.playerInventory, inventoryItem);
        inventoryItemDisplayInstances.Add(instance);
    }

    private void OnRemove_ToolBagItem(InventoryItem inventoryItem)
    {
        if (inventoryItem is QualityInventoryItem qualityInventoryItem)
        {
            QualityInventoryItemDisplay qualityInstance = qualityInventoryItemDisplayInstances.Find(i => i.data.inventoryItem == qualityInventoryItem);
            GameObject.Destroy(qualityInstance.gameObject);
            qualityInventoryItemDisplayInstances.Remove(qualityInstance);
            return;
        }
        InventoryItemDisplay instance = inventoryItemDisplayInstances.Find(i => i.data.inventoryItem == inventoryItem);
        GameObject.Destroy(instance.gameObject);
        inventoryItemDisplayInstances.Remove(instance);
    }
}