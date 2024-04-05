using UnityEngine;

public abstract class RoomContent
{
    public readonly ChunkRoom chunkRoom;
    public readonly Vector3 position;
    public readonly float rotation;
    public readonly bool canBeDestroyed;

    public RoomContent(ChunkRoom chunkRoom, Vector3 position, float rotation, bool canBeDestroyed)
    {
        this.chunkRoom = chunkRoom;
        this.position = position;
        this.rotation = rotation;
        this.canBeDestroyed = canBeDestroyed;
        chunkRoom.roomContents.Add(this);
    }

    public virtual void Destroy()
    {
        chunkRoom.roomContents.Remove(this);
    }

    public abstract Interactable CreateInteractable(Transform visual);
    public abstract bool EqualsVisual(Interactable component);
}