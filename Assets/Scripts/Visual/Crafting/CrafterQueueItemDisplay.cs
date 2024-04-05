using UnityEngine;
using UnityEngine.UI;

public class CrafterQueueItemDisplayData
{
    public readonly Crafter crafter;
    public readonly int index;

    public CrafterQueueItemDisplayData(Crafter crafter, int index)
    {
        this.crafter = crafter;
        this.index = index;
    }
}

public class CrafterQueueItemDisplay : DataDrivenUI<CrafterQueueItemDisplayData>
{
    [SerializeField] private Image itemIcon;
    [SerializeField] private Text amountRemainingText;
    [SerializeField] private Image craftProgressImage;
    [SerializeField] private Button moveUpButton;
    [SerializeField] private Button moveDownButton;
    [SerializeField] private Button cancelButton;

    private readonly EventVariable<CrafterQueueItemDisplay, CraftingItem> craftingItem;

    private CrafterQueueItemDisplay()
        : base()
    {
        craftingItem = new EventVariable<CrafterQueueItemDisplay, CraftingItem>(this, null);
    }

    private void Awake()
    {
        moveUpButton.onClick.AddListener(OnPress_MoveUpButton);
        moveDownButton.onClick.AddListener(OnPress_MoveDownButton);
        cancelButton.onClick.AddListener(OnPress_CancelButton);
        craftingItem.onValueChangeImmediate += OnValueChanged_CraftingItem;
    }

    protected override void OnValueChanged_Data(CrafterQueueItemDisplayData oldValue, CrafterQueueItemDisplayData newValue)
    {
        if(oldValue != null)
        {
            oldValue.crafter.onCraftingItemsSwapped -= OnEvent_CraftingItemsSwapped;
            oldValue.crafter.craftingQueue.onAdd -= OnAdd_CraftingQueue;
            oldValue.crafter.craftingQueue.onRemove -= OnRemove_CraftingQueue;
        }

        if(newValue != null)
        {
            newValue.crafter.onCraftingItemsSwapped += OnEvent_CraftingItemsSwapped;
            newValue.crafter.craftingQueue.onAdd += OnAdd_CraftingQueue;
            newValue.crafter.craftingQueue.onRemove += OnRemove_CraftingQueue;
            moveUpButton.gameObject.SetActive(data.index > 0);
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
        float amountFactor = 1f - ((float) newValue / craftingItem.value.craftingRecipe.CraftingTime);
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
        moveDownButton.gameObject.SetActive(data.index < data.crafter.craftingQueue.Count - 1);
    }

    private void OnPress_MoveUpButton()
    {
        data.crafter.MoveUpCraftingItem(craftingItem.value);
    }

    private void OnPress_MoveDownButton()
    {
        data.crafter.MoveDownCraftingItem(craftingItem.value);
    }

    private void OnPress_CancelButton()
    {
        data.crafter.CancelCraftingItem(craftingItem.value);
    }

    private void OnDestroy()
    {
        moveUpButton.onClick.RemoveListener(OnPress_MoveUpButton);
        moveDownButton.onClick.RemoveListener(OnPress_MoveDownButton);
        cancelButton.onClick.RemoveListener(OnPress_CancelButton);
        craftingItem.onValueChangeImmediate -= OnValueChanged_CraftingItem;
    }
}