using UnityEngine;
using SheetCodes;
using UnityEngine.UI;
using System.Collections.Generic;

public class CraftItemMenuData
{
    public readonly Crafter crafter;
    public readonly CraftingRecipeRecord craftingRecipe;

    public CraftItemMenuData(Crafter crafter, CraftingRecipeRecord craftingRecipe)
    {
        this.crafter = crafter;
        this.craftingRecipe = craftingRecipe;
    }
}

public class CraftItemMenu : DataDrivenUI<CraftItemMenuData>
{
    [SerializeField] private Text itemNameText;
    [SerializeField] private Text durationText;
    [SerializeField] private RectTransform inputContainer;
    [SerializeField] private RectTransform outputContainer;
    [SerializeField] private CraftInputItem craftInputItemPrefab;
    [SerializeField] private CraftOutputItem craftOutputItemPrefab;
    [SerializeField] private PositiveIntegerInputField craftAmountInputField;
    [SerializeField] private Button craftButton;
    [SerializeField] private CanvasGroup canvasGroup;

    private readonly List<CraftInputItem> craftInputItems;
    private readonly List<CraftOutputItem> craftOutputItems;

    protected CraftItemMenu()
        : base()
    {
        craftInputItems = new List<CraftInputItem>();
        craftOutputItems = new List<CraftOutputItem>();
    }

    private void Awake()
    {
        canvasGroup.Hide();
        craftAmountInputField.intValue.onValueChange += OnValueChanged_CraftAmountValue;
        craftButton.onClick.AddListener(OnPress_CraftButton);
    }

    protected override void OnValueChanged_Data(CraftItemMenuData oldValue, CraftItemMenuData newValue)
    {
        if (oldValue != null)
        {
            foreach (CraftingInputRecord input in oldValue.craftingRecipe.Input)
            {
                InventoryItemCollection item = oldValue.crafter.inventory.items[input.Item.Identifier];
                item.itemCount.onValueChange -= OnValueChanged_Input_ItemCount;
                item.reservedCount.onValueChange -= OnValueChanged_Input_ReservedCount;
            }

            foreach (CraftOutputItem instance in craftOutputItems)
                GameObject.Destroy(instance.gameObject);

            foreach (CraftInputItem instance in craftInputItems)
                GameObject.Destroy(instance.gameObject);

            oldValue.crafter.craftingQueue.onAdd -= OnAdd_CraftingQueue;
            oldValue.crafter.craftingQueue.onRemove -= OnRemove_CraftingQueue;

            craftOutputItems.Clear();
            craftInputItems.Clear();
            canvasGroup.Hide();
        }

        if (newValue != null)
        {
            foreach (CraftingInputRecord input in newValue.craftingRecipe.Input)
            {
                InventoryItemCollection item = newValue.crafter.inventory.items[input.Item.Identifier];
                item.itemCount.onValueChange += OnValueChanged_Input_ItemCount;
                item.reservedCount.onValueChange += OnValueChanged_Input_ReservedCount;
            }

            itemNameText.text = newValue.craftingRecipe.Name;
            SetDurationText();

            foreach (CraftingOutputRecord record in newValue.craftingRecipe.Output)
            {
                CraftOutputItem instance = GameObject.Instantiate(craftOutputItemPrefab, outputContainer);
                instance.data = record;
                craftOutputItems.Add(instance);
            }

            foreach (CraftingInputRecord record in newValue.craftingRecipe.Input)
            {
                CraftInputItem instance = GameObject.Instantiate(craftInputItemPrefab, inputContainer);
                instance.data = new CraftInputItemData(record, data.crafter.inventory, Mathf.Max(craftAmountInputField.intValue.value, 1));
                craftInputItems.Add(instance);
            }
            newValue.crafter.craftingQueue.onAdd += OnAdd_CraftingQueue;
            newValue.crafter.craftingQueue.onRemove += OnRemove_CraftingQueue;

            canvasGroup.Show();
            SetCraftButtonInteractability();
        }
    }

    private void OnAdd_CraftingQueue(CraftingItem item)
    {
        SetCraftButtonInteractability();
    }

    private void OnRemove_CraftingQueue(CraftingItem item)
    {
        SetCraftButtonInteractability();
    }

    private void OnValueChanged_Input_ReservedCount(long oldValue, long newValue)
    {
        SetCraftButtonInteractability();
    }

    private void OnValueChanged_Input_ItemCount(long oldValue, long newValue)
    {
        SetCraftButtonInteractability();
    }

    private void OnValueChanged_CraftAmountValue(int oldValue, int newValue)
    {
        if(newValue < 1)
        {
            craftAmountInputField.text = "1";
            return;
        }

        if(newValue > 1)
        {
            int maxCraftable = int.MaxValue;

            foreach (CraftingInputRecord input in data.craftingRecipe.Input)
            {
                int craftable = Mathf.FloorToInt(data.crafter.inventory.items[input.Item.Identifier].availableCount / input.Amount);
                maxCraftable = Mathf.Min(maxCraftable, craftable);
            }

            if(newValue > maxCraftable)
            {
                if (maxCraftable == 0)
                    craftAmountInputField.text = "1";
                else
                    craftAmountInputField.text = maxCraftable.ToString();
                return;
            }
        }

        foreach(CraftInputItem instance in craftInputItems)
            instance.data = new CraftInputItemData(instance.data.inputRecord, instance.data.inventory, Mathf.Max(newValue, 1));

        SetDurationText();
        SetCraftButtonInteractability();
    }
    

    private void SetDurationText()
    {
        durationText.text = data.crafter.GetCraftingDuration(data.craftingRecipe, craftAmountInputField.intValue.value).ToString("0.00");
    }

    private void SetCraftButtonInteractability()
    {
        if (data.crafter.craftingQueue.Count == 9)
        {
            craftButton.interactable = false;
            return;
        }

        foreach (CraftingInputRecord input in data.craftingRecipe.Input)
        {
            if (data.crafter.inventory.items[input.Item.Identifier].availableCount >= input.Amount * craftAmountInputField.intValue.value)
                continue;

            craftButton.interactable = false;
            return;
        }

        craftButton.interactable = true;
        return;
    }

    private void OnPress_CraftButton()
    {
        data.crafter.AddCraftingItem(CraftingRecipeItem.currentSelected.value.data, craftAmountInputField.intValue.value);
    }

    private void OnDestroy()
    {
        craftAmountInputField.intValue.onValueChange -= OnValueChanged_CraftAmountValue;
        craftButton.onClick.RemoveListener(OnPress_CraftButton);
    }
}