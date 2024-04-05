using SheetCodes;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryTransferDisplayData
{
    public readonly Inventory roomInventory;
    public readonly Inventory playerInventory;

    public InventoryTransferDisplayData(Inventory roomInventory, Inventory playerInventory)
    {
        this.roomInventory = roomInventory;
        this.playerInventory = playerInventory;
    }
}

public class InventoryTransferDisplay : DataDrivenUI<InventoryTransferDisplayData>
{
    [SerializeField] private InventoryTransferItemDisplay inventoryTransferItemDisplayPrefab;
    [SerializeField] private QualityInventoryTransferItemDisplay qualityInventoryTransferItemDisplayPrefab;
    [SerializeField] private RectTransform roomInventoryItemsContainer;
    [SerializeField] private RectTransform playerInventoryItemsContainer;
    [SerializeField] private InventoryFilterDisplay roomInventoryFilterDisplay;
    [SerializeField] private InventoryFilterDisplay playerInventoryFilterDisplay;
    [SerializeField] private PositiveIntegerInputField transferAmountInputField;
    [SerializeField] private InventoryInfo roomInventoryInfo;
    [SerializeField] private InventoryInfo playerInventoryInfo;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button depositAllButton;

    private readonly List<InventoryTransferItemDisplay> roomInventoryItemInstances;
    private readonly List<InventoryTransferItemDisplay> playerInventoryItemInstances;
    private readonly List<QualityInventoryTransferItemDisplay> qualityRoomInventoryItemInstances;
    private readonly List<QualityInventoryTransferItemDisplay> qualityPlayerInventoryItemInstances;

    private InventoryTransferDisplay()
        : base()
    {
        roomInventoryItemInstances = new List<InventoryTransferItemDisplay>();
        playerInventoryItemInstances = new List<InventoryTransferItemDisplay>();
        qualityRoomInventoryItemInstances = new List<QualityInventoryTransferItemDisplay>();
        qualityPlayerInventoryItemInstances = new List<QualityInventoryTransferItemDisplay>();
    }

    private void Awake()
    {
        closeButton.onClick.AddListener(OnPress_CloseButton);
        depositAllButton.onClick.AddListener(OnPress_DepositAll);
    }

    protected override void OnValueChanged_Data(InventoryTransferDisplayData oldValue, InventoryTransferDisplayData newValue)
    {
        if (oldValue != null)
        {
            foreach (InventoryTransferItemDisplay instance in roomInventoryItemInstances)
                GameObject.Destroy(instance.gameObject);

            foreach (InventoryTransferItemDisplay instance in playerInventoryItemInstances)
                GameObject.Destroy(instance.gameObject);

            foreach (QualityInventoryTransferItemDisplay instance in qualityPlayerInventoryItemInstances)
                GameObject.Destroy(instance.gameObject);

            foreach (QualityInventoryTransferItemDisplay instance in qualityRoomInventoryItemInstances)
                GameObject.Destroy(instance.gameObject);

            oldValue.playerInventory.filteredItems.onAdd -= OnAdd_PlayerInventory_FilteredItems;
            oldValue.playerInventory.filteredItems.onRemove -= OnRemove_PlayerInventory_FilteredItems;

            oldValue.roomInventory.filteredItems.onAdd -= OnAdd_RoomInventory_FilteredItems;
            oldValue.roomInventory.filteredItems.onRemove -= OnRemove_RoomInventory_FilteredItems;

            roomInventoryItemInstances.Clear();
            playerInventoryItemInstances.Clear();
            qualityRoomInventoryItemInstances.Clear();
            qualityPlayerInventoryItemInstances.Clear();
            CursorController.instance.cursorVisibleCount.value--;
        }

        if(newValue != null)
        {
            roomInventoryInfo.data = newValue.roomInventory;
            playerInventoryInfo.data = newValue.playerInventory;

            roomInventoryFilterDisplay.data = newValue.roomInventory.filter;
            playerInventoryFilterDisplay.data = newValue.playerInventory.filter;

            newValue.playerInventory.filteredItems.onAdd += OnAdd_PlayerInventory_FilteredItems;
            newValue.playerInventory.filteredItems.onRemove += OnRemove_PlayerInventory_FilteredItems;

            newValue.roomInventory.filteredItems.onAdd += OnAdd_RoomInventory_FilteredItems;
            newValue.roomInventory.filteredItems.onRemove += OnRemove_RoomInventory_FilteredItems;

            foreach (InventoryItem item in newValue.playerInventory.filteredItems)
                OnAdd_PlayerInventory_FilteredItems(item);

            foreach (InventoryItem item in newValue.roomInventory.filteredItems)
                OnAdd_RoomInventory_FilteredItems(item);

            CursorController.instance.cursorVisibleCount.value++;
        }
        else
        {
            roomInventoryInfo.data = null;
            playerInventoryInfo.data = null;

            roomInventoryFilterDisplay.data = null;
            playerInventoryFilterDisplay.data = null;

        }
    }

    private void OnPress_CloseButton()
    {
        IngameScreenManager.instance.ShowScreen_Hud();
    }

    private void OnPress_DepositAll()
    {
        for(int i = data.playerInventory.filteredItems.Count - 1; i >= 0; i--)
        {
            InventoryItem item = data.playerInventory.filteredItems[i];

            if (item.isLocked.value)
                continue;

            long amount = item.availableCount;

            InventoryItemCollection itemCollection = data.roomInventory.items[item.itemRecord.Identifier];
            if (data.roomInventory.TryGetItemLimit(itemCollection, out long itemLimit))
                amount = Math.Min(amount, itemLimit);
            item.itemCount.value -= amount;

            if (item is QualityInventoryItem qualityItem)
                itemCollection.inventoryItems[qualityItem.qualityRecord.Identifier].itemCount.value += amount;
            else
                itemCollection.inventoryItems[ItemQualityIdentifier.None].itemCount.value += amount;
        }
    }

    private void OnAdd_PlayerInventory_FilteredItems(InventoryItem item)
    {
        if (item is QualityInventoryItem qualityInventoryItem)
        {
            QualityInventoryTransferItemDisplay instance = GameObject.Instantiate(qualityInventoryTransferItemDisplayPrefab, playerInventoryItemsContainer);
            instance.data = new QualityInventoryTransferItemDisplayData(qualityInventoryItem);
            instance.onLeftPress_TransferButton += OnLeftPress_TransferButton_QualityPlayerInventory;
            instance.onRightPress_TransferButton += OnRightPress_TransferButton_QualityPlayerInventory;
            qualityPlayerInventoryItemInstances.Add(instance);
        }
        else
        {
            InventoryTransferItemDisplay instance = GameObject.Instantiate(inventoryTransferItemDisplayPrefab, playerInventoryItemsContainer);
            instance.data = new InventoryTransferItemDisplayData(item);
            instance.onLeftPress_TransferButton += OnLeftPress_TransferButton_PlayerInventory;
            instance.onRightPress_TransferButton += OnRightPress_TransferButton_PlayerInventory;
            playerInventoryItemInstances.Add(instance);
        }
    }

    private void OnRemove_PlayerInventory_FilteredItems(InventoryItem item)
    {
        if (item is QualityInventoryItem qualityInventoryItem)
        {
            QualityInventoryTransferItemDisplay instance = qualityPlayerInventoryItemInstances.Find(i => i.data.inventoryItem == qualityInventoryItem);
            instance.onLeftPress_TransferButton -= OnLeftPress_TransferButton_QualityPlayerInventory;
            instance.onRightPress_TransferButton -= OnRightPress_TransferButton_QualityPlayerInventory;
            GameObject.Destroy(instance.gameObject);
            qualityPlayerInventoryItemInstances.Remove(instance);
        }
        else
        {
            InventoryTransferItemDisplay instance = playerInventoryItemInstances.Find(i => i.data.inventoryItem == item);
            instance.onLeftPress_TransferButton -= OnLeftPress_TransferButton_PlayerInventory;
            instance.onRightPress_TransferButton -= OnRightPress_TransferButton_PlayerInventory;
            GameObject.Destroy(instance.gameObject);
            playerInventoryItemInstances.Remove(instance);
        }
    }

    private void OnLeftPress_TransferButton_QualityPlayerInventory(QualityInventoryTransferItemDisplay source)
    {
        TransferItems(source.data.inventoryItem, data.roomInventory);
    }

    private void OnLeftPress_TransferButton_PlayerInventory(InventoryTransferItemDisplay source)
    {
        TransferItems(source.data.inventoryItem, data.roomInventory);
    }

    private void OnRightPress_TransferButton_QualityPlayerInventory(QualityInventoryTransferItemDisplay source)
    {
        source.data.inventoryItem.isLocked.value = !source.data.inventoryItem.isLocked.value;
    }

    private void OnRightPress_TransferButton_PlayerInventory(InventoryTransferItemDisplay source)
    {
        source.data.inventoryItem.isLocked.value = !source.data.inventoryItem.isLocked.value;
    }

    private void OnAdd_RoomInventory_FilteredItems(InventoryItem item)
    {
        if (item is QualityInventoryItem qualityInventoryItem)
        {
            QualityInventoryTransferItemDisplay instance = GameObject.Instantiate(qualityInventoryTransferItemDisplayPrefab, roomInventoryItemsContainer);
            instance.data = new QualityInventoryTransferItemDisplayData(qualityInventoryItem);
            instance.onLeftPress_TransferButton += OnLeftPress_TransferButton_QualityRoomInventory;
            qualityRoomInventoryItemInstances.Add(instance);
        }
        else
        {
            InventoryTransferItemDisplay instance = GameObject.Instantiate(inventoryTransferItemDisplayPrefab, roomInventoryItemsContainer);
            instance.data = new InventoryTransferItemDisplayData(item);
            instance.onLeftPress_TransferButton += OnLeftPress_TransferButton_RoomInventory;
            roomInventoryItemInstances.Add(instance);
        }
    }

    private void OnRemove_RoomInventory_FilteredItems(InventoryItem item)
    {
        if (item is QualityInventoryItem qualityInventoryItem)
        {
            QualityInventoryTransferItemDisplay instance = qualityRoomInventoryItemInstances.Find(i => i.data.inventoryItem == qualityInventoryItem);
            instance.onLeftPress_TransferButton -= OnLeftPress_TransferButton_QualityRoomInventory;
            GameObject.Destroy(instance.gameObject);
            qualityRoomInventoryItemInstances.Remove(instance);
        }
        else
        {
            InventoryTransferItemDisplay instance = roomInventoryItemInstances.Find(i => i.data.inventoryItem == item);
            instance.onLeftPress_TransferButton -= OnLeftPress_TransferButton_RoomInventory;
            GameObject.Destroy(instance.gameObject);
            roomInventoryItemInstances.Remove(instance);
        }
    }

    private void OnLeftPress_TransferButton_QualityRoomInventory(QualityInventoryTransferItemDisplay source)
    {
        TransferItems(source.data.inventoryItem, data.playerInventory);
    }

    private void OnLeftPress_TransferButton_RoomInventory(InventoryTransferItemDisplay source)
    {
        TransferItems(source.data.inventoryItem, data.playerInventory);
    }

    private void TransferItems(InventoryItem item, Inventory targetInventory)
    {
        if (item.isLocked.value)
            return;

        long amount;
        if (Input.GetKey(KeyCode.LeftShift))
            amount = item.availableCount;
        else
            amount = Math.Min(item.availableCount, transferAmountInputField.intValue.value);

        InventoryItemCollection itemCollection = targetInventory.items[item.itemRecord.Identifier];
        if (targetInventory.TryGetItemLimit(itemCollection, out long itemLimit))
            amount = Math.Min(amount, itemLimit);

        item.itemCount.value -= amount;

        if (item is QualityInventoryItem qualityItem)
            itemCollection.inventoryItems[qualityItem.qualityRecord.Identifier].itemCount.value += amount;
        else
            itemCollection.inventoryItems[ItemQualityIdentifier.None].itemCount.value += amount;
    }

    private void Destroy()
    {
        closeButton.onClick.RemoveListener(OnPress_CloseButton);
        depositAllButton.onClick.RemoveListener(OnPress_DepositAll);
    }
}