public class PlayerInventory
{
    public readonly Inventory inventory;
    public readonly Crafter crafter;
    public readonly ToolBag[] toolBags;
    public readonly EventVariable<PlayerInventory, ToolBag> selectedToolBag;

    public PlayerInventory()
    {
        inventory = new Inventory(new InventoryFilter(), true);
        crafter = new Crafter(inventory, 100, true);
        toolBags = new ToolBag[6];

        for (int i = 0; i < toolBags.Length; i++)
        {
            ToolBag toolBag = new ToolBag(i);
            toolBag.selected.onValueChangeSource += OnValueChanged_ToolBag_Selected;
            toolBags[i] = toolBag;
        }

        selectedToolBag = new EventVariable<PlayerInventory, ToolBag>(this, toolBags[0]);
        selectedToolBag.onValueChangeImmediate += OnValueChanged_SelectedToolBag;
    }

    public void AddToToolBag(QualityInventoryItem item)
    {
        selectedToolBag.value.toolBagItems.Add(item);
    }

    public void RemoveFromToolBag(QualityInventoryItem item)
    {
        selectedToolBag.value.toolBagItems.Remove(item);
    }

    private void OnValueChanged_ToolBag_Selected(ToolBag source, bool oldValue, bool newValue)
    {
        if (newValue)
            selectedToolBag.value = source;
    }

    private void OnValueChanged_SelectedToolBag(ToolBag oldValue, ToolBag newValue)
    {
        if (oldValue != null)
        {
            oldValue.selected.value = false;

            oldValue.toolBagItems.onAdd -= OnAdd_SelectedToolBag_InventoryItem;
            oldValue.toolBagItems.onRemove -= OnRemove_SelectedToolBag_InventoryItem;

            foreach (QualityInventoryItem toolBagItem in oldValue.toolBagItems)
                toolBagItem.toolBagEquipped.value = false;
        }

        if (newValue != null)
        {
            newValue.selected.value = true;

            newValue.toolBagItems.onAdd += OnAdd_SelectedToolBag_InventoryItem;
            newValue.toolBagItems.onRemove += OnRemove_SelectedToolBag_InventoryItem;

            foreach (QualityInventoryItem toolBagItem in newValue.toolBagItems)
                toolBagItem.toolBagEquipped.value = true;
        }
    }

    private void OnAdd_SelectedToolBag_InventoryItem(QualityInventoryItem inventoryItem)
    {
        inventoryItem.toolBagEquipped.value = true;
    }

    private void OnRemove_SelectedToolBag_InventoryItem(QualityInventoryItem inventoryItem)
    {
        inventoryItem.toolBagEquipped.value = false;
    }

}