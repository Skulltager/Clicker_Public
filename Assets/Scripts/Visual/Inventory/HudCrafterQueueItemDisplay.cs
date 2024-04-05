using UnityEngine;
using UnityEngine.UI;

public class HudCrafterQueueItemDisplayData
{
    public readonly Crafter crafter;
    public readonly int index;

    public HudCrafterQueueItemDisplayData(Crafter crafter, int index)
    {
        this.crafter = crafter;
        this.index = index;
    }
}

public class HudCrafterQueueItemDisplay : DataDrivenUI<HudCrafterQueueItemDisplayData>
{
    [SerializeField] private Image itemIcon;
    [SerializeField] private Text amountRemainingText;
    [SerializeField] private Image craftProgressImage;

    private readonly EventVariable<HudCrafterQueueItemDisplay, CraftingItem> craftingItem;

    private HudCrafterQueueItemDisplay()
        : base()
    {
        craftingItem = new EventVariable<HudCrafterQueueItemDisplay, CraftingItem>(this, null);
    }

    private void Awake()
    {
        craftingItem.onValueChangeImmediate += OnValueChanged_CraftingItem;
    }

    protected override void OnValueChanged_Data(HudCrafterQueueItemDisplayData oldValue, HudCrafterQueueItemDisplayData newValue)
    {
        if (oldValue != null)
        {
            oldValue.crafter.onCraftingItemsSwapped -= OnEvent_CraftingItemsSwapped;
            oldValue.crafter.craftingQueue.onAdd -= OnAdd_CraftingQueue;
            oldValue.crafter.craftingQueue.onRemove -= OnRemove_CraftingQueue;
        }

        if (newValue != null)
        {
            newValue.crafter.onCraftingItemsSwapped += OnEvent_CraftingItemsSwapped;
            newValue.crafter.craftingQueue.onAdd += OnAdd_CraftingQueue;
            newValue.crafter.craftingQueue.onRemove += OnRemove_CraftingQueue;
            SetCraftingItem();
        }
    }

    private void OnValueChanged_CraftingItem(CraftingItem oldValue, CraftingItem newValue)
    {
        if (oldValue != null)
        {
            oldValue.amountLeft.onValueChange -= OnValueChanged_CraftingItem_AmountLeft;
            oldValue.craftingAmountLeft.onValueChange -= OnValueChanged_CraftingItem_CraftingAmountLeft;
        }

        if (newValue != null)
        {
            newValue.amountLeft.onValueChangeImmediate += OnValueChanged_CraftingItem_AmountLeft;
            newValue.craftingAmountLeft.onValueChangeImmediate += OnValueChanged_CraftingItem_CraftingAmountLeft;
            itemIcon.sprite = newValue.craftingRecipe.Icon;
        }
    }

    private void OnValueChanged_CraftingItem_AmountLeft(long oldValue, long newValue)
    {
        amountRemainingText.text = newValue.ToString();
    }

    private void OnValueChanged_CraftingItem_CraftingAmountLeft(long oldValue, long newValue)
    {
        float amountFactor = 1f - ((float)newValue / craftingItem.value.craftingRecipe.CraftingTime);
        craftProgressImage.fillAmount = amountFactor;
    }

    private void OnAdd_CraftingQueue(CraftingItem item)
    {
        SetCraftingItem();
    }

    private void OnRemove_CraftingQueue(CraftingItem item)
    {
        SetCraftingItem();
    }

    private void OnEvent_CraftingItemsSwapped()
    {
        SetCraftingItem();
    }

    private void SetCraftingItem()
    {
        if (data.crafter.craftingQueue.Count <= data.index)
            return;

        craftingItem.value = data.crafter.craftingQueue[data.index];
    }

    private void OnDestroy()
    {
        craftingItem.onValueChangeImmediate -= OnValueChanged_CraftingItem;
    }
}