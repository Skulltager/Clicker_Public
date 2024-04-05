
using System.Collections.Generic;
using UnityEngine;

public class MapChunkVisualizer : DataDrivenBehaviour<MapChunk>
{
    [SerializeField] private ChunkRoomVisual chunkRoomVisualPrefab = default;
    [SerializeField] private Transform chunkRoomContainer = default;

    private readonly List<ChunkRoomVisual> chunkRoomVisuals;

    private MapChunkVisualizer()
    {
        chunkRoomVisuals = new List<ChunkRoomVisual>();
    }

    protected override void OnValueChanged_Data(MapChunk oldValue, MapChunk newValue)
    {
        if (oldValue != null)
        {
            oldValue.finishedRooms.onAdd -= OnAdd_FinishedRoom;
            foreach (ChunkRoomVisual chunkRoomVisual in chunkRoomVisuals)
                GameObject.Destroy(chunkRoomVisual.gameObject);

            chunkRoomVisuals.Clear();
        }

        if (newValue != null)
        {
            newValue.finishedRooms.onAdd += OnAdd_FinishedRoom;
            foreach (ChunkRoom chunkRoom in newValue.finishedRooms)
                OnAdd_FinishedRoom(chunkRoom);
        }
    }


    private void OnAdd_FinishedRoom(ChunkRoom chunkRoom)
    {
        ChunkRoomVisual instance = GameObject.Instantiate(chunkRoomVisualPrefab, chunkRoomContainer);
        instance.data = chunkRoom;
        chunkRoomVisuals.Add(instance);
    }
}