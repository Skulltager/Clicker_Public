using UnityEngine;
using UnityEngine.UI;

public class InventoryQualityButton : DataDrivenUI<InventoryQualityFilter>
{
    [SerializeField] private Button unselectButton;
    [SerializeField] private Button selectButton;
    [SerializeField] private Text qualityLevel;
    [SerializeField] private GameObject selectedContent;
    [SerializeField] private GameObject unselectedContent;

    private void Awake()
    {
        unselectButton.onClick.AddListener(OnPress_UnselectButton);
        selectButton.onClick.AddListener(OnPress_SelectButton);
    }

    protected override void OnValueChanged_Data(InventoryQualityFilter oldValue, InventoryQualityFilter newValue)
    {
        if (oldValue != null)
        {
            oldValue.selected.onValueChange -= OnValueChanged_Selected;
        }

        if (newValue != null)
        {
            qualityLevel.text = newValue.record.QualityText;
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