using UnityEngine;
using UnityEngine.UI;

public class CraftingMenu : DataDrivenUI<Crafter>
{
    [SerializeField] private Button closeButton;
    [SerializeField] private CraftItemMenu craftItemMenu;
    [SerializeField] private CrafterQueueDisplay crafterQueueDisplay;

    private void Awake()
    {
        closeButton.onClick.AddListener(OnPress_CloseButton);
        CraftingRecipeItem.currentSelected.onValueChange += OnValueChanged_SelectedCraftingItem;
    }

    private void OnValueChanged_SelectedCraftingItem(CraftingRecipeItem oldValue, CraftingRecipeItem newValue)
    {
        SetChildData();
    }

    protected override void OnValueChanged_Data(Crafter oldValue, Crafter newValue)
    {
        if (oldValue != null)
        {
            CursorController.instance.cursorVisibleCount.value--;
        }

        if (newValue != null)
        {
            CursorController.instance.cursorVisibleCount.value++;
            SetChildData();
        }
    }

    private void SetChildData()
    {
        if (CraftingRecipeItem.currentSelected.value == null)
        {
            craftItemMenu.data = null;
            return;
        }

        if(data == null)
        {
            craftItemMenu.data = null;
            return;
        }

        craftItemMenu.data = new CraftItemMenuData(data, CraftingRecipeItem.currentSelected.value.data);
        crafterQueueDisplay.data = data;
    }

    private void OnPress_CloseButton()
    {
        IngameScreenManager.instance.ShowScreen_Hud();
    }

    private void OnDestroy()
    {
        CraftingRecipeItem.currentSelected.onValueChange -= OnValueChanged_SelectedCraftingItem;
        closeButton.onClick.RemoveListener(OnPress_CloseButton);
    }
}