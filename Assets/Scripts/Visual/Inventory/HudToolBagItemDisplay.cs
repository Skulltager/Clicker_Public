using UnityEngine;
using UnityEngine.UI;

public class HudToolBagItemDisplay : DataDrivenUI<ToolBagItemCategory>
{
    [SerializeField] private Image icon;
    [SerializeField] private Text qualityText;
    [SerializeField] private Image durabilityRemainingImage;
    [SerializeField] private Text amountRemainingText;
    [SerializeField] private Image itemCategoryImage;
    [SerializeField] private GameObject durabilityContainer;
    [SerializeField] private GameObject equippedItemContainer;
    [SerializeField] private GameObject nothingEquippedItemContainer;

    protected override void OnValueChanged_Data(ToolBagItemCategory oldValue, ToolBagItemCategory newValue)
    {
        if (oldValue != null)
        {
            oldValue.equippedItem.onValueChangeImmediate -= OnValueChanged_EquippedItem;
        }

        if (newValue != null)
        {
            newValue.equippedItem.onValueChangeImmediate += OnValueChanged_EquippedItem;
        }
    }

    private void OnValueChanged_EquippedItem(QualityInventoryItem oldValue, QualityInventoryItem newValue)
    {
        if (oldValue != null)
        {
            oldValue.itemCount.onValueChange -= OnValueChanged_ItemCount;
            oldValue.reservedCount.onValueChange -= OnValueChanged_ReservedCount;
            oldValue.durabilityLeft.onValueChange -= OnValueChanged_DurabilityLeft;
        }

        if (newValue != null)
        {
            newValue.itemCount.onValueChange += OnValueChanged_ItemCount;
            newValue.reservedCount.onValueChange += OnValueChanged_ReservedCount;
            newValue.durabilityLeft.onValueChange += OnValueChanged_DurabilityLeft;

            icon.sprite = newValue.itemRecord.Icon;
            qualityText.text = newValue.qualityRecord.QualityText;
            itemCategoryImage.sprite = newValue.itemRecord.ItemCategory.Icon;

            SetRemainingAmountText();
            SetDurabilityImage();
        }
        else
        {
            equippedItemContainer.SetActive(false);
            durabilityContainer.SetActive(true);
        }
    }

    private void OnValueChanged_DurabilityLeft(int oldValue, int newValue)
    {
        SetDurabilityImage();
    }

    private void OnValueChanged_ReservedCount(long oldValue, long newValue)
    {
        SetRemainingAmountText();
        SetDurabilityImage();
    }

    private void OnValueChanged_ItemCount(long oldValue, long newValue)
    {
        SetRemainingAmountText();
        SetDurabilityImage();
    }

    private void SetRemainingAmountText()
    {
        long available = data.equippedItem.eventStackValue.availableCount;
        nothingEquippedItemContainer.SetActive(available == 0);
        equippedItemContainer.SetActive(available > 0);
        durabilityContainer.SetActive(available > 0);
        amountRemainingText.text = available.ToString();
    }

    private void SetDurabilityImage()
    {
        if (data.equippedItem.value.availableCount == 0)
        {
            durabilityRemainingImage.fillAmount = 0;
            return;
        }

        durabilityRemainingImage.fillAmount = (float)data.equippedItem.value.durabilityLeft.value / data.equippedItem.value.statsRecord.Durability;
    }
}