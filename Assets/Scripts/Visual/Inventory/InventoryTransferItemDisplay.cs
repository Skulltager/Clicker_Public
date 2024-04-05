using System;
using UnityEngine;
using UnityEngine.UI;

public class InventoryTransferItemDisplayData
{
    public readonly InventoryItem inventoryItem;

    public InventoryTransferItemDisplayData(InventoryItem inventoryItem)
    {
        this.inventoryItem = inventoryItem;
    }
}

public class InventoryTransferItemDisplay : DataDrivenUI<InventoryTransferItemDisplayData>
{
    [SerializeField] private GuiClickHandler transferButton;
    [SerializeField] private Text amountText;
    [SerializeField] private Text reservedText;
    [SerializeField] private Image icon;
    [SerializeField] private GameObject reservedContent;
    [SerializeField] private GameObject lockedContent;

    public event Action<InventoryTransferItemDisplay> onLeftPress_TransferButton;
    public event Action<InventoryTransferItemDisplay> onRightPress_TransferButton;

    private void Awake()
    {
        transferButton.onLeft += OnLeftPress_TransferButton;
        transferButton.onRight += OnRightPress_TransferButton;
    }

    private void OnLeftPress_TransferButton()
    {
        if (onLeftPress_TransferButton != null)
            onLeftPress_TransferButton(this);
    }

    private void OnRightPress_TransferButton()
    {
        if (onRightPress_TransferButton != null)
            onRightPress_TransferButton(this);
    }

    protected override void OnValueChanged_Data(InventoryTransferItemDisplayData oldValue, InventoryTransferItemDisplayData newValue)
    {
        if (oldValue != null)
        {
            oldValue.inventoryItem.itemCount.onValueChange -= OnValueChanged_ItemCount;
            oldValue.inventoryItem.reservedCount.onValueChange -= OnValueChanged_ReservedCount;
            oldValue.inventoryItem.isLocked.onValueChange -= OnValueChanged_IsLocked;
        }

        if (newValue != null)
        {
            newValue.inventoryItem.itemCount.onValueChangeImmediate += OnValueChanged_ItemCount;
            newValue.inventoryItem.reservedCount.onValueChangeImmediate += OnValueChanged_ReservedCount;
            newValue.inventoryItem.isLocked.onValueChangeImmediate += OnValueChanged_IsLocked;
            icon.sprite = newValue.inventoryItem.itemRecord.Icon;
        }
    }

    private void OnValueChanged_IsLocked(bool oldValue, bool newValue)
    {
        lockedContent.SetActive(newValue);
    }

    private void OnValueChanged_ReservedCount(long oldValue, long newValue)
    {
        reservedContent.SetActive(newValue > 0);
        reservedText.text = newValue.ToString();
        SetRemainingAmountText();
    }

    private void OnValueChanged_ItemCount(long oldValue, long newValue)
    {
        SetRemainingAmountText();
    }

    private void SetRemainingAmountText()
    {
        amountText.text = data.inventoryItem.availableCount.ToString();
    }

    private void OnDestroy()
    {
        transferButton.onLeft -= OnLeftPress_TransferButton;
        transferButton.onRight -= OnRightPress_TransferButton;
    }
}