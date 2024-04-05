using UnityEngine;
using SheetCodes;
using UnityEngine.UI;

public class CraftingRecipeItem : DataDrivenUI<CraftingRecipeRecord>
{
    public readonly static EventVariable<CraftingRecipeItem, CraftingRecipeItem> currentSelected;

    [SerializeField] private Text categoryText;
    [SerializeField] private Button selectButton;
    [SerializeField] private GameObject selectedContent;
    [SerializeField] private GameObject unselectedContent;

    static CraftingRecipeItem()
    {
        currentSelected = new EventVariable<CraftingRecipeItem, CraftingRecipeItem>(null, null);
    }

    protected override void OnValueChanged_Data(CraftingRecipeRecord oldValue, CraftingRecipeRecord newValue)
    {
        if (newValue != null)
        {
            categoryText.text = newValue.Name;
        }
    }

    private void Awake()
    {
        selectButton.onClick.AddListener(OnPress_SelectButton);
        currentSelected.onValueChangeImmediate += OnValueChanged_CurrentSelected;
    }

    private void OnValueChanged_CurrentSelected(CraftingRecipeItem oldValue, CraftingRecipeItem newValue)
    {
        if (newValue == this)
        {
            selectedContent.SetActive(true);
            unselectedContent.SetActive(false);
        }
        else
        {
            selectedContent.SetActive(false);
            unselectedContent.SetActive(true);
        }
    }

    private void OnPress_SelectButton()
    {
        currentSelected.value = this;
    }

    private void OnDestroy()
    {
        if (currentSelected.value == this)
            currentSelected.value = null;

        selectButton.onClick.RemoveListener(OnPress_SelectButton);
        currentSelected.onValueChange -= OnValueChanged_CurrentSelected;
    }
}