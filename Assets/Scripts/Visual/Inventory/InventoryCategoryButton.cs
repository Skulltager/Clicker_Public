using UnityEngine;
using UnityEngine.UI;

public class InventoryCategoryButton : DataDrivenUI<InventoryCategoryFilter>
{
    [SerializeField] private Button unselectButton;
    [SerializeField] private Button selectButton;
    [SerializeField] private Image icon;
    [SerializeField] private GameObject selectedContent;
    [SerializeField] private GameObject unselectedContent;

    private void Awake()
    {
        unselectButton.onClick.AddListener(OnPress_UnselectButton);
        selectButton.onClick.AddListener(OnPress_SelectButton);
    }

    protected override void OnValueChanged_Data(InventoryCategoryFilter oldValue, InventoryCategoryFilter newValue)
    {
        if (oldValue != null)
        {
            oldValue.selected.onValueChange -= OnValueChanged_Selected;
        }

        if (newValue != null)
        {
            icon.sprite = newValue.record.Icon;
            newValue.selected.onValueChangeImmediate += OnValueChanged_Selected;
        }
    }

    private void OnValueChanged_Selected(bool oldValue, bool newValue)
    {
        if (newValue)
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

    private void OnPress_UnselectButton()
    {
        data.selected.value = false;
    }

    private void OnPress_SelectButton()
    {
        data.selected.value = true;
    }

    private void OnDestroy()
    {
        unselectButton.onClick.RemoveListener(OnPress_UnselectButton);
        selectButton.onClick.RemoveListener(OnPress_SelectButton);
    }
}