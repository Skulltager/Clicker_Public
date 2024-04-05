using SheetCodes;

public class CraftingItem
{
    private readonly Crafter crafter;
    public readonly long amount;
    public readonly CraftingRecipeRecord craftingRecipe;
    public readonly EventVariable<CraftingItem, long> amountLeft;
    public readonly EventVariable<CraftingItem, long> craftingAmountLeft;
    public int index;

    public CraftingItem(Crafter crafter, CraftingRecipeRecord craftingRecipe, long amount, int index)
    {
        this.crafter = crafter;
        this.craftingRecipe = craftingRecipe;
        this.amount = amount;
        this.index = index;
        amountLeft = new EventVariable<CraftingItem, long>(this, amount);
        craftingAmountLeft = new EventVariable<CraftingItem, long>(this, craftingRecipe.CraftingTime);
    }
    
    public (bool, long) CraftTick(long craftAmount)
    {
        if(craftAmount < craftingAmountLeft.value)
        {
            craftingAmountLeft.value -= craftAmount;
            return (false, 0);
        }

        craftAmount -= craftingAmountLeft.value;
        long amountDifference = craftAmount / craftingRecipe.CraftingTime + 1;

        if (amountDifference < amountLeft.value)
        {
            amountLeft.value -= amountDifference;
            craftAmount -= (amountDifference - 1) * craftingRecipe.CraftingTime;
            craftingAmountLeft.value = craftingRecipe.CraftingTime - craftAmount;
            FinishCraft(amountDifference);
            return (false, 0);
        }

        FinishCraft(amountLeft.value);
        craftAmount -= (amountLeft.value - 1) * craftingRecipe.CraftingTime;
        amountLeft.value = 0;
        craftingAmountLeft.value = 0;
        return (true, craftAmount);
    }

    public void FinishCraft(long amount)
    {
        foreach (CraftingInputRecord input in craftingRecipe.Input)
        {
            InventoryItemCollection item = crafter.inventory.items[input.Item.Identifier];
            item.UseReservedItems(amount * input.Amount);
        }

        for (int i = 0; i < amount; i++)
        {
            foreach (CraftingOutputRecord output in craftingRecipe.Output)
            {
                if (UnityEngine.Random.value > output.Chance)
                    continue;

                InventoryItemCollection item = crafter.inventory.items[output.Item.Identifier];
                long itemAmount = UnityEngine.Random.Range(output.MinAmount, output.MaxAmount);
                long oldItemCount = item.itemCount.value;
                item.AddItems(itemAmount, crafter.showNotifications);
            }
        }
    }

    public long GetTotalCraftingLeft()
    {
        return craftingRecipe.CraftingTime * (amountLeft.value - 1) + craftingAmountLeft.value;
    }
}
