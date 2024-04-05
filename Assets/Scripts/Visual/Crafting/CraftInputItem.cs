using UnityEngine;
using SheetCodes;
using UnityEngine.UI;

public class CraftInputItemData
{
    public readonly Inventory inventory;
    public readonly CraftingInputRecord inputRecord;
    public readonly long amount;

    public CraftInputItemData(CraftingInputRecord inputRecord, Inventory inventory, long amount)
    {
        this.inputRecord = inputRecord;
        this.inventory = inventory;
        this.amount = amount;
    }
}

public class CraftInputItem : DataDrivenUI<CraftInputItemData>
{
    [SerializeField] private Text amountText;
    [SerializeField] private Image itemIcon;
    [SerializeField] private Color notEnoughColor;
    [SerializeField] private Color enoughColor;

    public readonly EventVariable<CraftInputItem, bool> requirementsMet;

    private CraftInputItem()
        :base()
    {
        requirementsMet = new EventVariable<CraftInputItem, bool>(this, false);
    }

    private void Awake()
    {
        requirementsMet.onValueChangeImmediate += OnValueChanged_RequirementsMet;
    }

    protected override void OnValueChanged_Data(CraftInputItemData oldValue, CraftInputItemData newValue)
    {
        if(oldValue != null)
        {
            InventoryItemCollection inventoryItem = oldValue.inventory.items[oldValue.inputRecord.Item.Identifier];
            inventoryItem.itemCount.onValueChange -= OnValueChanged_ItemCount;
            inventoryItem.reservedCount.onValueChange -= OnValueChanged_ReservedCount;
        }

        if(newValue != null)
        {
            InventoryItemCollection inventoryItem = newValue.inventory.items[newValue.inputRecord.Item.Identifier];
            inventoryItem.itemCount.onValueChange += OnValueChanged_ItemCount;
            inventoryItem.reservedCount.onValueChange += OnValueChanged_ReservedCount;

            itemIcon.sprite = newValue.inputRecord.Item.Icon;
            SetRequirementsMet();
        }
    }

    private void OnValueChanged_ItemCount(long oldValue, long newValue)
    {
        SetRequirementsMet();
    }

    private void OnValueChanged_ReservedCount(long oldValue, long newValue)
    {
        SetRequirementsMet();
    }

    private void SetRequirementsMet()
    {
        InventoryItemCollection inventoryItem = data.inventory.items[data.inputRecord.Item.Identifier];
        requirementsMet.value = data.inputRecord.Amount * data.amount <= inventoryItem.availableCount;

        long amountNeeded = data.inputRecord.Amount * data.amount;
        amountText.text = string.Format("{0} ({1})", amountNeeded, inventoryItem.availableCount);
    }

    private void OnValueChanged_RequirementsMet(bool oldValue, bool newValue)
    {
        amountText.color = newValue ? enoughColor : notEnoughColor;
    }

    private void OnDestroy()
    {
        requirementsMet.onValueChange -= OnValueChanged_RequirementsMet;
    }
}