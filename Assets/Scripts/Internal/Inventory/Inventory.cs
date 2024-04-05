using SheetCodes;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    public readonly Dictionary<ItemIdentifier, InventoryItemCollection> items;
    public readonly EventList<InventoryItem> existingItems;
    public readonly EventList<InventoryItem> filteredItems;
    public readonly EventVariable<Inventory, long> maxWeight;
    public readonly EventVariable<Inventory, long> currentWeight;
    public readonly EventVariable<Inventory, bool> infiniteWeight;
    public readonly EventVariable<Inventory, bool> overWeight;
    public readonly InventoryFilter filter;
    public readonly bool canOverload;

    public Inventory(InventoryFilter filter, bool canOverload)
    {
        this.filter = filter;
        this.canOverload = canOverload;
        existingItems = new EventList<InventoryItem>();
        filteredItems = new EventList<InventoryItem>();

        maxWeight = new EventVariable<Inventory, long>(this, 50000);
        currentWeight = new EventVariable<Inventory, long>(this, 0);
        infiniteWeight = new EventVariable<Inventory, bool>(this, false);
        overWeight = new EventVariable<Inventory, bool>(this, false);

        maxWeight.onValueChange += OnValueChanged_MaxWeight;
        currentWeight.onValueChange += OnValueChanged_CurrentWeight;
        infiniteWeight.onValueChange += OnValueChanged_InfiniteWeight;


        existingItems.onAdd += OnAdd_ExistingItem;
        existingItems.onRemove += OnRemove_ExistingItem;

        items = new Dictionary<ItemIdentifier, InventoryItemCollection>();

        ItemIdentifier[] itemIdentifiers = Enum.GetValues(typeof(ItemIdentifier)) as ItemIdentifier[];
        
        foreach(ItemIdentifier identifier in itemIdentifiers)
        {
            if (identifier == ItemIdentifier.None)
                continue;

            ItemRecord record = identifier.GetRecord();
            InventoryItemCollection itemCollection = new InventoryItemCollection(record, filter);
            items.Add(identifier, itemCollection);
            foreach (InventoryItem inventoryItem in itemCollection.inventoryItems.Values)
                inventoryItem.itemCount.onValueChangeImmediateSource += OnValueChanged_InventoryItem_ItemCount;
        }
    }

    public bool TryGetItemLimit(InventoryItemCollection itemCollection, out long itemLimit)
    {
        if (infiniteWeight.value)
        {
            itemLimit = 0;
            return false;
        }

        itemLimit = (maxWeight.value - currentWeight.value) / itemCollection.itemRecord.Weight;
        return true;
    }

    private void OnValueChanged_InfiniteWeight(bool oldValue, bool newValue)
    {
        SetOverweight();
    }

    private void OnValueChanged_MaxWeight(long oldValue, long newValue)
    {
        SetOverweight();
    }

    private void OnValueChanged_CurrentWeight(long oldValue, long newValue)
    {
        SetOverweight();
    }

    private void SetOverweight()
    {
        overWeight.value = currentWeight.value > maxWeight.value && !infiniteWeight.value;
    }

    private void OnValueChanged_InventoryItem_ItemCount(InventoryItem source, long oldValue, long newValue)
    {
        currentWeight.value += (newValue - oldValue) * source.itemRecord.Weight;

        if (oldValue == 0 && newValue > 0)
            existingItems.Add(source);

        else if (oldValue > 0 && newValue == 0)
            existingItems.Remove(source);
    }

    private void OnAdd_ExistingItem(InventoryItem item)
    {
        item.matchesFilter.onValueChangeImmediateSource += OnValueChanged_ExistingItem_MatchesFilter;
    }

    private void OnRemove_ExistingItem(InventoryItem item)
    {
        item.matchesFilter.onValueChangeImmediateSource -= OnValueChanged_ExistingItem_MatchesFilter;
    }

    private void OnValueChanged_ExistingItem_MatchesFilter(InventoryItem source, bool oldValue, bool newValue)
    {
        if (oldValue)
            filteredItems.Remove(source);

        if (newValue)
            filteredItems.Add(source);
    }
}