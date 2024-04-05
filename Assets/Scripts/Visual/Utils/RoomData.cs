using UnityEngine;

public class RoomData: MonoBehaviour
{
    public readonly EventVariable<RoomData, ChunkRoom> chunkRoom;

    private RoomData()
    {
        chunkRoom = new EventVariable<RoomData, ChunkRoom>(this, null);
    }
}
