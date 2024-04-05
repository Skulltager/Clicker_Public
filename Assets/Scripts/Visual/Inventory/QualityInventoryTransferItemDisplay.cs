using System;
using UnityEngine;
using UnityEngine.UI;

public class QualityInventoryTransferItemDisplayData
{
    public readonly QualityInventoryItem inventoryItem;

    public QualityInventoryTransferItemDisplayData(QualityInventoryItem inventoryItem)
    {
        this.inventoryItem = inventoryItem;
    }
}

public class QualityInventoryTransferItemDisplay : DataDrivenUI<QualityInventoryTransferItemDisplayData>
{
    [SerializeField] private GuiClickHandler transferButton;
    [SerializeField] private Text amountText;
    [SerializeField] private Text reservedText;
    [SerializeField] private Image icon;
    [SerializeField] private GameObject reservedContent;
    [SerializeField] private Image durabilityRemainingImage;
    [SerializeField] private Text qualityText;
    [SerializeField] private GameObject lockedContent;

    public event Action<QualityInventoryTransferItemDisplay> onLeftPress_TransferButton;
    public event Action<QualityInventoryTransferItemDisplay> onRightPress_TransferButton;

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

    protected override void OnValueChanged_Data(QualityInventoryTransferItemDisplayData oldValue, QualityInventoryTransferItemDisplayData newValue)
    {
        if (oldValue != null)
        {
            oldValue.inventoryItem.itemCount.onValueChange -= OnValueChanged_ItemCount;
            oldValue.inventoryItem.reservedCount.onValueChange -= OnValueChanged_ReservedCount;
            oldValue.inventoryItem.durabilityLeft.onValueChange -= OnValueChanged_DurabilityLeft;
            oldValue.inventoryItem.isLocked.onValueChange -= OnValueChanged_IsLocked;
        }

        if (newValue != null)
        {
            newValue.inventoryItem.itemCount.onValueChangeImmediate += OnValueChanged_ItemCount;
            newValue.inventoryItem.reservedCount.onValueChangeImmediate += OnValueChanged_ReservedCount;
            newValue.inventoryItem.durabilityLeft.onValueChangeImmediate += OnValueChanged_DurabilityLeft;
            newValue.inventoryItem.isLocked.onValueChangeImmediate += OnValueChanged_IsLocked;
            qualityText.text = newValue.inventoryItem.qualityRecord.QualityText;
            icon.sprite = newValue.inventoryItem.itemRecord.Icon;
        }
    }

    private void OnValueChanged_IsLocked(bool oldValue, bool newValue)
    {
        lockedContent.SetActive(newValue);
    }

    private void OnValueChanged_DurabilityLeft(int oldValue, int newValue)
    {
        durabilityRemainingImage.fillAmount = (float)newValue / data.inventoryItem.statsRecord.Durability;
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