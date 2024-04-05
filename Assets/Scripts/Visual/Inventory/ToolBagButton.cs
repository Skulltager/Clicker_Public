using UnityEngine;
using UnityEngine.UI;

public class ToolBagButton : DataDrivenUI<ToolBag>
{
    [SerializeField] private Button button;
    [SerializeField] private GameObject selectedContent;
    [SerializeField] private GameObject unselectedContent;
    [SerializeField] private Text indexText;

    private void Awake()
    {
        button.onClick.AddListener(OnPress_Button);
    }

    protected override void OnValueChanged_Data(ToolBag oldValue, ToolBag newValue)
    {
        if (oldValue != null)
            oldValue.selected.onValueChange -= OnValueChanged_Selected;

        if (newValue != null)
        {
            newValue.selected.onValueChangeImmediate += OnValueChanged_Selected;
            indexText.text = (newValue.index + 1).ToString();
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

    private void OnPress_Button()
    {
        data.selected.value = true;
    }

    private void Destroy()
    {
        button.onClick.RemoveListener(OnPress_Button);
    }
}