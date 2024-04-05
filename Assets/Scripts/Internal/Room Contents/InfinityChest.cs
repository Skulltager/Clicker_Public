using UnityEngine;
using SheetCodes;

public class InfinityChest : RoomContent
{
    public InfinityChest(ChunkRoom chunkRoom, Vector3 position, float rotation)
        : base(chunkRoom, position, rotation, false)
    {
        chunkRoom.CreateInventory();
        chunkRoom.inventory.infiniteWeight.value = true;
    }

    public override Interactable CreateInteractable(Transform container)
    {
        InfinityChestVisual visual = GameObject.Instantiate(PrefabCollectionIdentifier.Prefabs.GetRecord().InfiniteChest, container);
        visual.data.value = this;
        visual.transform.position = position;
        visual.transform.rotation = Quaternion.Euler(0, rotation, 0);
        visual.Initialize(chunkRoom);
        return visual;
    }

    public override bool EqualsVisual(Interactable component)
    {
        if (component is not InfinityChestVisual visual)
            return false;

        if (visual.data.value != this)
            return false;

        return true;
    }

    public override void Destroy()
    {
        chunkRoom.inventory.infiniteWeight.value = false;
        base.Destroy();
    }
}