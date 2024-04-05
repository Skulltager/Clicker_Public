using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public readonly EventVariable<Interactable, ChunkRoom> chunkRoom;

    protected Interactable()
    {
        chunkRoom = new EventVariable<Interactable, ChunkRoom>(this, null);
    }

    public abstract bool CheckHitObject(Vector2 texCoord);
    public abstract void Interact(PlayerInventory playerInventory, ItemPickupLocation itemPickupLocation);
}