using UnityEngine;

public class Hud : DataDrivenUI<PlayerInventory>
{
    [SerializeField] private HudToolBagDisplay hudToolBagDisplay;
    [SerializeField] private HudCrafterQueueDisplay hudCrafterQueueDisplay;

    protected override void OnValueChanged_Data(PlayerInventory oldValue, PlayerInventory newValue)
    {
        if(oldValue != null)
        {
            oldValue.selectedToolBag.onValueChange -= OnValueChanged_SelectedToolBag;
        }

        if(newValue != null)
        {
            newValue.selectedToolBag.onValueChangeImmediate += OnValueChanged_SelectedToolBag;
            hudCrafterQueueDisplay.data = newValue.crafter;
        }
        else
        {
            hudCrafterQueueDisplay.data = null;
        }
    }

    private void OnValueChanged_SelectedToolBag(ToolBag oldValue, ToolBag newValue)
    {
        hudToolBagDisplay.data = newValue;
    }    
}