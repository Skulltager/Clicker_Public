using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventoryDisplay : DataDrivenUI<PlayerInventory>
{
    [SerializeField] private Button closeButton;
    [SerializeField] private ToolBagButton toolBagButtonPrefab;
    [SerializeField] private RectTransform toolBagButtonContainer;
    [SerializeField] private FilteredInventoryDisplay inventoryDisplay;
    [SerializeField] private ToolBagDisplay toolBagDisplay;
    [SerializeField] private InventoryInfo playerInventoryInfo;

    private readonly List<ToolBagButton> toolBagButtonInstances;

    private PlayerInventoryDisplay()
        :base()
    {
        toolBagButtonInstances = new List<ToolBagButton>();
    }

    private void Awake()
    {
        closeButton.onClick.AddListener(OnPress_CloseButton);
    }

    protected override void OnValueChanged_Data(PlayerInventory oldValue, PlayerInventory newValue)
    {
        if (oldValue != null)
        {
            foreach (ToolBagButton instance in toolBagButtonInstances)
                GameObject.Destroy(instance.gameObject);

            toolBagButtonInstances.Clear();
            oldValue.selectedToolBag.onValueChangeImmediate -= OnValueChanged_PlayerInventory_SelectedToolBag;

            CursorController.instance.cursorVisibleCount.value--;
        }

        if (newValue != null)
        {
            foreach(ToolBag toolBag in newValue.toolBags)
            {
                ToolBagButton instance = GameObject.Instantiate(toolBagButtonPrefab, toolBagButtonContainer);
                instance.data = toolBag;
                toolBagButtonInstances.Add(instance);
            }
            newValue.selectedToolBag.onValueChangeImmediate += OnValueChanged_PlayerInventory_SelectedToolBag;

            playerInventoryInfo.data = newValue.inventory;
            CursorController.instance.cursorVisibleCount.value++;
        }
        else
        {
            playerInventoryInfo.data = null;
        }    

        inventoryDisplay.data = newValue;
    }

    private void OnValueChanged_PlayerInventory_SelectedToolBag(ToolBag oldValue, ToolBag newValue)
    {
        if (newValue != null)
            toolBagDisplay.data = new ToolBagDisplayData(data, newValue);
        else
            toolBagDisplay.data = null;
    }

    private void OnPress_CloseButton()
    {
        IngameScreenManager.instance.ShowScreen_Hud();
    }

    private void OnDestroy()
    {
        closeButton.onClick.RemoveListener(OnPress_CloseButton);
    }
}