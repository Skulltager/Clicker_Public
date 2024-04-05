using SheetCodes;
using System;

public class Crafter
{
    public readonly Inventory inventory;
    public readonly EventVariable<Crafter, int> craftingPerTick;
    public readonly EventList<CraftingItem> craftingQueue;
    public event Action onCraftingItemsSwapped;
    public readonly bool showNotifications;

    public Crafter(Inventory inventory, int craftingPerTick, bool showNotifications)
    {
        this.inventory = inventory;
        this.craftingPerTick = new EventVariable<Crafter, int>(this, craftingPerTick);
        craftingQueue = new EventList<CraftingItem>();
        this.showNotifications = showNotifications;
    }

    public void AddCraftingItem(CraftingRecipeRecord craftingRecipe, int amount)
    {
        craftingQueue.Add(new CraftingItem(this, craftingRecipe, amount, craftingQueue.Count));

        foreach (CraftingInputRecord input in craftingRecipe.Input)
        {
            int itemAmount = input.Amount * amount;
            inventory.items[input.Item.Identifier].ReserveItems(itemAmount);
        }
    }

    public bool CanCraftItem(CraftingRecipeRecord craftingRecipe, int amount)
    {
        foreach (CraftingInputRecord input in craftingRecipe.Input)
        {
            int itemAmount = input.Amount * amount;
            if (inventory.items[input.Item.Identifier].availableCount < itemAmount)
                return false;
        }

        return true;
    }

    public void CancelCraftingItem(CraftingItem item)
    {
        foreach (CraftingInputRecord input in item.craftingRecipe.Input)
        {
            long itemAmount = input.Amount * item.amountLeft.value;
            inventory.items[input.Item.Identifier].UnreserveItems(itemAmount);
        }

        craftingQueue.RemoveAt(item.index);
        for(int i = item.index; i < craftingQueue.Count; i++)
            craftingQueue[i].index = i;
    }

    public void MoveUpCraftingItem(CraftingItem item)
    {
        item.index--;

        CraftingItem otherItem = craftingQueue[item.index];
        otherItem.index++;

        craftingQueue[otherItem.index] = otherItem;
        craftingQueue[item.index] = item;

        if (onCraftingItemsSwapped != null)
            onCraftingItemsSwapped();
    }

    public void MoveDownCraftingItem(CraftingItem item)
    {
        item.index++;

        CraftingItem otherItem = craftingQueue[item.index];
        otherItem.index--;

        craftingQueue[otherItem.index] = otherItem;
        craftingQueue[item.index] = item;

        if (onCraftingItemsSwapped != null)
            onCraftingItemsSwapped();
    }

    public void CraftTick()
    {
        long craftingLeft = craftingPerTick.value;
        while (craftingQueue.Count > 0 && craftingLeft > 0)
        {
            bool finishedCraft;
            (finishedCraft, craftingLeft) = craftingQueue[0].CraftTick(craftingLeft);

            if (finishedCraft)
            {
                craftingQueue.RemoveAt(0);
                for(int i = 0; i < craftingQueue.Count; i++)
                    craftingQueue[i].index = i;
            }
        }
    }

    public float GetCraftingDuration(CraftingRecipeRecord craftingRecipe, int amount)
    {
        long totalTicks = craftingRecipe.CraftingTime * amount;
        return totalTicks / (craftingPerTick.value * 60f);
    }
} 