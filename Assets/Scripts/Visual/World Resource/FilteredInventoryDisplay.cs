using System.Collections.Generic;
using UnityEngine;

public class FilteredInventoryDisplay : DataDrivenUI<PlayerInventory>
{
    [SerializeField] private RectTransform inventoryItemContainer;
    [SerializeField] private InventoryItemDisplay inventoryItemDisplayPrefab;
    [SerializeField] private QualityInventoryItemDisplay qualityInventoryItemDisplayPrefab;
    [SerializeField] private InventoryFilterDisplay filterDisplay;

    private List<InventoryItemDisplay> inventoryItemDisplayInstances;
    private List<QualityInventoryItemDisplay> qualityInventoryItemDisplayInstances;

    private FilteredInventoryDisplay()
    {
        inventoryItemDisplayInstances = new List<InventoryItemDisplay>();
        qualityInventoryItemDisplayInstances = new List<QualityInventoryItemDisplay>();
    }

    protected override void OnValueChanged_Data(PlayerInventory oldValue, PlayerInventory newValue)
    {
        if(oldValue != null)
        {
            foreach (InventoryItemDisplay instance in inventoryItemDisplayInstances)
                GameObject.Destroy(instance.gameObject);

            foreach (QualityInventoryItemDisplay instance in qualityInventoryItemDisplayInstances)
                GameObject.Destroy(instance.gameObject);

            inventoryItemDisplayInstances.Clear();
            qualityInventoryItemDisplayInstances.Clear();

            oldValue.inventory.filteredItems.onAdd -= OnAdd_FilteredItem;
            oldValue.inventory.filteredItems.onRemove -= OnRemove_FilteredItem;
            filterDisplay.data = null;
        }

        if(newValue != null)
        {
            foreach (InventoryItem inventoryItem in newValue.inventory.filteredItems)
                OnAdd_FilteredItem(inventoryItem);

            newValue.inventory.filteredItems.onAdd += OnAdd_FilteredItem;
            newValue.inventory.filteredItems.onRemove += OnRemove_FilteredItem;
            filterDisplay.data = newValue.inventory.filter;
        }
    }

    private void OnAdd_FilteredItem(InventoryItem inventoryItem)
    {
        if(inventoryItem is QualityInventoryItem qualityInventoryItem)
        {
            QualityInventoryItemDisplay qualityInstance = GameObject.Instantiate(qualityInventoryItemDisplayPrefab, inventoryItemContainer);
            qualityInstance.data = new QualityInventoryItemDisplayData(data, qualityInventoryItem);
            qualityInventoryItemDisplayInstances.Add(qualityInstance);
            return;
        }

        InventoryItemDisplay instance = GameObject.Instantiate(inventoryItemDisplayPrefab, inventoryItemContainer);
        instance.data = new InventoryItemDisplayData(data, inventoryItem);
        inventoryItemDisplayInstances.Add(instance);
    }

    private void OnRemove_FilteredItem(InventoryItem inventoryItem)
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