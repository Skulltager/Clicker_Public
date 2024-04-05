using SheetCodes;
using System;
using System.Collections.Generic;
using System.Linq;

public class InventoryItemCollection
{
    public readonly ItemRecord itemRecord;
    public readonly Dictionary<ItemQualityIdentifier, InventoryItem> inventoryItems;
    public readonly EventVariable<InventoryItemCollection, long> itemCount;
    public readonly EventVariable<InventoryItemCollection, long> reservedCount;
    public long availableCount => itemCount.value - reservedCount.value;

    private readonly ItemQualityIdentifier[] qualityIdentifiers;

    public InventoryItemCollection(ItemRecord itemRecord, InventoryFilter filter)
    {
        this.itemRecord = itemRecord;
        inventoryItems = new Dictionary<ItemQualityIdentifier, InventoryItem>();
        itemCount = new EventVariable<InventoryItemCollection, long>(this, 0);
        reservedCount = new EventVariable<InventoryItemCollection, long>(this, 0);

        ItemQualityIdentifier[] qualityIdentifiers = Enum.GetValues(typeof(ItemQualityIdentifier)) as ItemQualityIdentifier[];

        if (itemRecord.ItemCategory.HasQuality)
        {
            for (int j = 1; j < qualityIdentifiers.Length; j++)
            {
                ItemQualityIdentifier qualityIdentifier = qualityIdentifiers[j];
                ItemQualityRecord qualityRecord = qualityIdentifier.GetRecord();
                InventoryItem inventoryItem = new QualityInventoryItem(itemRecord, 2, filter, qualityRecord);
                inventoryItem.itemCount.onValueChangeImmediateSource += OnValueChanged_InventoryItem_ItemCount;
                inventoryItem.reservedCount.onValueChangeImmediateSource += OnValueChanged_InventoryItem_ReservedCount;
                inventoryItems.Add(qualityIdentifier, inventoryItem);
            }
            this.qualityIdentifiers = qualityIdentifiers.Where(i => i != ItemQualityIdentifier.None).ToArray();
        }
        else
        {
            ItemQualityIdentifier qualityIdentifier = qualityIdentifiers[0];
            InventoryItem inventoryItem = new InventoryItem(itemRecord, 0, filter);
            inventoryItem.itemCount.onValueChangeImmediateSource += OnValueChanged_InventoryItem_ItemCount;
            inventoryItem.reservedCount.onValueChangeImmediateSource += OnValueChanged_InventoryItem_ReservedCount;
            inventoryItems.Add(qualityIdentifier, inventoryItem);
            this.qualityIdentifiers = new ItemQualityIdentifier[] { ItemQualityIdentifier.None };
        }
    }

    public void AddItems(long amount, bool showNotification)
    {
        InventoryItem item = inventoryItems[qualityIdentifiers[0]];
        if (showNotification)
            ItemPickupNotificationManager.instance.ShowItemNotification(item, amount);

        //Got to figure out how to assign the proper quality
        item.itemCount.value += amount;
    }

    public void ReserveItems(long amount)
    {
        for(int i = 0; i < qualityIdentifiers.Length; i++)
        {
            ItemQualityIdentifier qualityIdentifier = qualityIdentifiers[i];
            InventoryItem inventoryItem = inventoryItems[qualityIdentifier];
            long maxAmount = Math.Min(inventoryItem.availableCount, amount);
            inventoryItem.reservedCount.value += maxAmount;
            amount -= maxAmount;

            if (amount == 0)
                return;
        }
    }

    public void UseReservedItems(long amount)
    {
        for (int i = 0; i < qualityIdentifiers.Length; i++)
        {
            ItemQualityIdentifier qualityIdentifier = qualityIdentifiers[i];
            InventoryItem inventoryItem = inventoryItems[qualityIdentifier];
            long maxAmount = Math.Min(inventoryItem.reservedCount.value, amount);
            inventoryItem.reservedCount.value -= maxAmount;
            inventoryItem.itemCount.value -= maxAmount;
            amount -= maxAmount;

            if (amount == 0)
                return;
        }
    }

    public void UnreserveItems(long amount)
    {
        for (int i = qualityIdentifiers.Length - 1; i >= 0; i++)
        {
            ItemQualityIdentifier qualityIdentifier = qualityIdentifiers[i];
            InventoryItem inventoryItem = inventoryItems[qualityIdentifier];
            long maxAmount = Math.Min(inventoryItem.reservedCount.value, amount);
            inventoryItem.reservedCount.value -= maxAmount;
            amount -= maxAmount;

            if (amount == 0)
                return;
        }
    }

    private void OnValueChanged_InventoryItem_ItemCount(InventoryItem source, long oldValue, long newValue)
    {
        itemCount.value += newValue - oldValue;
    }

    private void OnValueChanged_InventoryItem_ReservedCount(InventoryItem source, long oldValue, long newValue)
    {
        reservedCount.value += newValue - oldValue;
    }
}