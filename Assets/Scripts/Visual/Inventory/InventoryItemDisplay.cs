using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItemDisplayData
{
    public readonly PlayerInventory playerInventory;
    public readonly InventoryItem inventoryItem;

    public InventoryItemDisplayData(PlayerInventory playerInventory, InventoryItem inventoryItem)
    {
        this.playerInventory = playerInventory;
        this.inventoryItem = inventoryItem;
    }
}

public class InventoryItemDisplay : DataDrivenUI<InventoryItemDisplayData>
{
    [SerializeField] private GuiClickHandler interactButton;
    [SerializeField] private Text amountText;
    [SerializeField] private Text reservedText;
    [SerializeField] private Image icon;
    [SerializeField] private GameObject reservedContent;
    [SerializeField] private GameObject lockedContent;

    private void Awake()
    {
        interactButton.onLeft += OnLeftPress_InteractButton;
        interactButton.onRight += OnRightPress_InteractButton;
    }

    protected override void OnValueChanged_Data(InventoryItemDisplayData oldValue, InventoryItemDisplayData newValue)
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

    private void OnLeftPress_InteractButton()
    {
        if(Input.GetKey(KeyCode.D))
        {
            data.inventoryItem.itemCount.value = 0;
            return;
        }
    }

    private void OnRightPress_InteractButton()
    {
        data.inventoryItem.isLocked.value = !data.inventoryItem.isLocked.value;
    }

    private void OnDestroy()
    {
        interactButton.onLeft -= OnLeftPress_InteractButton;
        interactButton.onRight -= OnRightPress_InteractButton;
    }
}