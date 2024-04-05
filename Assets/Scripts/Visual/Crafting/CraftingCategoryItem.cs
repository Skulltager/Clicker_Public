using UnityEngine;
using SheetCodes;
using UnityEngine.UI;

public class CraftingCategoryItem : DataDrivenUI<CraftingCategoryRecord>
{
    public readonly static EventVariable<CraftingCategoryItem, CraftingCategoryItem> currentSelected;

    [SerializeField] private Text categoryText;
    [SerializeField] private Button selectButton;
    [SerializeField] private GameObject selectedContent;
    [SerializeField] private GameObject unselectedContent;

    static CraftingCategoryItem()
    {
        currentSelected = new EventVariable<CraftingCategoryItem, CraftingCategoryItem>(null, null);
    }

    protected override void OnValueChanged_Data(CraftingCategoryRecord oldValue, CraftingCategoryRecord newValue)
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

    private void OnValueChanged_CurrentSelected(CraftingCategoryItem oldValue, CraftingCategoryItem newValue)
    {
        if(newValue == this)
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