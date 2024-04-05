
using System.Collections.Generic;

public struct ChunkRoomSet
{
    public readonly List<ChunkRoom> chunkRooms;
    public readonly int setIndex;

    public ChunkRoomSet(ChunkRoom initialRoom, int setIndex)
    {
        chunkRooms = new List<ChunkRoom>();
        chunkRooms.Add(initialRoom);
        this.setIndex = setIndex;
    }
}