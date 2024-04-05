using UnityEngine;

public class DoorData : MonoBehaviour
{
    public readonly EventVariable<DoorData, ChunkRoom> fromChunkRoom;
    public readonly EventVariable<DoorData, ChunkRoom> toChunkRoom;

    private DoorData()
    {
        fromChunkRoom = new EventVariable<DoorData, ChunkRoom>(this, null);
        toChunkRoom = new EventVariable<DoorData, ChunkRoom>(this, null);
    }
}
