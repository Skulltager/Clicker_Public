using UnityEngine;
using UnityEngine.UI;

public class QualityInventoryItemDisplayData
{
    public readonly PlayerInventory playerInventory;
    public readonly QualityInventoryItem inventoryItem;

    public QualityInventoryItemDisplayData(PlayerInventory playerInventory, QualityInventoryItem inventoryItem)
    {
        this.playerInventory = playerInventory;
        this.inventoryItem = inventoryItem;
    }
}

public class QualityInventoryItemDisplay : DataDrivenUI<QualityInventoryItemDisplayData>
{
    [SerializeField] private GuiClickHandler equipButton;
    [SerializeField] private GuiClickHandler unequipButton;
    [SerializeField] private Text qualityText;
    [SerializeField] private Text amountText;
    [SerializeField] private Text reservedText;
    [SerializeField] private Image icon;
    [SerializeField] private GameObject reservedContent;
    [SerializeField] private GameObject equippedContent;
    [SerializeField] private GameObject unequippedContent;
    [SerializeField] private GameObject lockedContent;
    [SerializeField] private Image durabilityRemainingImage;

    private void Awake()
    {
        equipButton.onLeft += OnLeftPress_EquipButton;
        equipButton.onRight += OnRightPress_EquipButton;
        unequipButton.onLeft += OnLeftPress_UnequipButton;
        unequipButton.onRight += OnRightPress_UnequipButton;
    }

    protected override void OnValueChanged_Data(QualityInventoryItemDisplayData oldValue, QualityInventoryItemDisplayData newValue)
    {
        if (oldValue != null)
        {
            oldValue.inventoryItem.itemCount.onValueChange -= OnValueChanged_ItemCount;
            oldValue.inventoryItem.reservedCount.onValueChange -= OnValueChanged_ReservedCount;
            oldValue.inventoryItem.toolBagEquipped.onValueChange -= OnValueChanged_Equipped;
            oldValue.inventoryItem.durabilityLeft.onValueChange -= OnValueChanged_DurabilityLeft;
            oldValue.inventoryItem.isLocked.onValueChange -= OnValueChanged_IsLocked;
        }

        if (newValue != null)
        {
            newValue.inventoryItem.itemCount.onValueChangeImmediate += OnValueChanged_ItemCount;
            newValue.inventoryItem.reservedCount.onValueChangeImmediate += OnValueChanged_ReservedCount;
            newValue.inventoryItem.toolBagEquipped.onValueChangeImmediate += OnValueChanged_Equipped;
            newValue.inventoryItem.durabilityLeft.onValueChangeImmediate += OnValueChanged_DurabilityLeft;
            newValue.inventoryItem.isLocked.onValueChangeImmediate += OnValueChanged_IsLocked;
            icon.sprite = newValue.inventoryItem.itemRecord.Icon;
            qualityText.text = newValue.inventoryItem.qualityRecord.QualityText;
        }
    }

    private void OnValueChanged_IsLocked(bool oldValue, bool newValue)
    {
        lockedContent.SetActive(newValue);
    }

    private void OnValueChanged_DurabilityLeft(int oldValue, int newValue)
    {
        SetDurabilityImage();
    }

    private void OnValueChanged_Equipped(bool oldValue, bool newValue)
    {
        if (newValue)
        {
            equippedContent.SetActive(true);
            unequippedContent.SetActive(false);
        }
        else
        {
            equippedContent.SetActive(false);
            unequippedContent.SetActive(true);
        }
    }

    private void OnValueChanged_ReservedCount(long oldValue, long newValue)
    {
        reservedContent.SetActive(newValue > 0);
        reservedText.text = newValue.ToString();
        SetDurabilityImage();
        SetRemainingAmountText();
    }

    private void OnValueChanged_ItemCount(long oldValue, long newValue)
    {
        SetDurabilityImage();
        SetRemainingAmountText();
    }

    private void SetDurabilityImage()
    {
        if (data.inventoryItem.availableCount == 0)
        {
            durabilityRemainingImage.fillAmount = 0;
            return;
        }

        durabilityRemainingImage.fillAmount = (float)data.inventoryItem.durabilityLeft.value / data.inventoryItem.statsRecord.Durability;
    }

    private void SetRemainingAmountText()
    {
        amountText.text = data.inventoryItem.availableCount.ToString();
    }

    private void OnRightPress_EquipButton()
    {
        data.inventoryItem.isLocked.value = !data.inventoryItem.isLocked.value;
    }

    private void OnRightPress_UnequipButton()
    {
        data.inventoryItem.isLocked.value = !data.inventoryItem.isLocked.value;
    }

    private void OnLeftPress_EquipButton()
    {
        if (data.inventoryItem.itemRecord.ItemCategory.CanEquip && Input.GetKey(KeyCode.LeftShift))
        {
            InventoryItemCollection inventoryItemCollection = data.playerInventory.inventory.items[data.inventoryItem.itemRecord.Identifier];

            foreach (InventoryItem inventoryItem in inventoryItemCollection.inventoryItems.Values)
            {
                if (inventoryItem is not QualityInventoryItem qualityInventoryItem)
                    continue;

                if (qualityInventoryItem.itemCount.value == 0)
                    continue;

                if (qualityInventoryItem.toolBagEquipped.value)
                    continue;

                data.playerInventory.AddToToolBag(qualityInventoryItem);
            }
            return;
        }

        if (Input.GetKey(KeyCode.D))
        {
            data.inventoryItem.itemCount.value = 0;
            return;
        }

        if (data.inventoryItem.itemRecord.ItemCategory.CanEquip)
            data.playerInventory.AddToToolBag(data.inventoryItem);
    }

    private void OnLeftPress_UnequipButton()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            InventoryItemCollection inventoryItemCollection = data.playerInventory.inventory.items[data.inventoryItem.itemRecord.Identifier];

            foreach (InventoryItem inventoryItem in inventoryItemCollection.inventoryItems.Values)
            {
                if (inventoryItem is not QualityInventoryItem qualityInventoryItem)
                    continue;

                if (!qualityInventoryItem.toolBagEquipped.value)
                    continue;

                data.playerInventory.RemoveFromToolBag(qualityInventoryItem);
            }
            return;
        }

        if (Input.GetKey(KeyCode.D))
        {
            data.inventoryItem.itemCount.value = 0;
            return;
        }

        data.playerInventory.RemoveFromToolBag(data.inventoryItem);
    }

    private void OnDestroy()
    {
        equipButton.onLeft -= OnLeftPress_EquipButton;
        equipButton.onRight -= OnRightPress_EquipButton;
        unequipButton.onLeft -= OnLeftPress_UnequipButton;
        unequipButton.onRight -= OnRightPress_UnequipButton;
    }
}