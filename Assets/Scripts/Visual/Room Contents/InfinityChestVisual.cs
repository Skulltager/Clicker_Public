using UnityEngine;

public class InfinityChestVisual : Interactable
{
    public readonly EventVariable<InfinityChestVisual, InfinityChest> data;

    private InfinityChestVisual()
    {
        data = new EventVariable<InfinityChestVisual, InfinityChest>(this, null);
    }

    public void Initialize(ChunkRoom chunkRoom)
    {
        this.chunkRoom.value = chunkRoom;
    }

    public override bool CheckHitObject(Vector2 texCoord)
    {
        return true;
    }

    public override void Interact(PlayerInventory playerInventory, ItemPickupLocation itemPickupLocation)
    {
        IngameScreenManager.instance.ShowScreen_InventoryTransferDisplay(data.value.chunkRoom.inventory, playerInventory.inventory);
    }
}