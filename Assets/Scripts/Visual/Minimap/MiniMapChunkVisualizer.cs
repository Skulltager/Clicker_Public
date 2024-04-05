
using System.Collections.Generic;
using UnityEngine;

public class MiniMapChunkVisualizer : DataDrivenBehaviour<MapChunk>
{
    [SerializeField] private MiniChunkRoomVisual miniChunkRoomVisualPrefab = default;
    [SerializeField] private Transform miniChunkRoomContainer = default;

    private readonly List<MiniChunkRoomVisual> miniChunkRoomVisuals;
    private readonly List<MiniChunkRoomVisual> miniChunkRoomsRendering;

    public readonly EventVariable<MiniMapChunkVisualizer, bool> shouldRender;
    public readonly EventVariable<MiniMapChunkVisualizer, MinMaxVector2> minMaxVector;

    private MiniMapChunkVisualizer()
    {
        miniChunkRoomVisuals = new List<MiniChunkRoomVisual>();
        miniChunkRoomsRendering = new List<MiniChunkRoomVisual>();
        shouldRender = new EventVariable<MiniMapChunkVisualizer, bool>(this, false);
        minMaxVector = new EventVariable<MiniMapChunkVisualizer, MinMaxVector2>(this, default);
    }

    protected override void OnValueChanged_Data(MapChunk oldValue, MapChunk newValue)
    {
        if (oldValue != null)
        {
            oldValue.finishedRooms.onAdd -= OnAdd_FinishedRoom;
            foreach (MiniChunkRoomVisual chunkRoomVisual in miniChunkRoomVisuals)
            {
                chunkRoomVisual.shouldRender.onValueChangeSource -= OnValueChanged_MiniChunkRoom_ShouldRender;
                GameObject.Destroy(chunkRoomVisual.gameObject);
            }
            miniChunkRoomsRendering.Clear();
            miniChunkRoomVisuals.Clear();
            shouldRender.value = false;
        }

        if (newValue != null)
        {
            newValue.finishedRooms.onAdd += OnAdd_FinishedRoom;
            foreach (ChunkRoom chunkRoom in newValue.finishedRooms)
                OnAdd_FinishedRoom(chunkRoom);
        }
    }

    private void OnValueChanged_MiniChunkRoom_ShouldRender(MiniChunkRoomVisual source, bool oldValue, bool newValue)
    {
        if (oldValue)
            miniChunkRoomsRendering.Remove(source);

        if (newValue)
            miniChunkRoomsRendering.Add(source);

        RecalculateBounds();
        shouldRender.value = miniChunkRoomsRendering.Count > 0;
    }

    private void OnAdd_FinishedRoom(ChunkRoom chunkRoom)
    {
        MiniChunkRoomVisual instance = GameObject.Instantiate(miniChunkRoomVisualPrefab, miniChunkRoomContainer);
        instance.shouldRender.onValueChangeImmediateSource += OnValueChanged_MiniChunkRoom_ShouldRender;
        instance.data = chunkRoom;
        miniChunkRoomVisuals.Add(instance);
    }

    private void RecalculateBounds()
    {
        if (miniChunkRoomsRendering.Count == 0)
        {
            minMaxVector.value = MinMaxVector2.CreateEmpty();
            return;
        }

        MinMaxVector2 bounds = miniChunkRoomsRendering[0].data.bounds;

        for (int i = 1; i < miniChunkRoomsRendering.Count; i++)
            bounds = MinMaxVector2.AssignHighestOffsets(bounds, miniChunkRoomsRendering[i].data.bounds);

        minMaxVector.value = bounds;
    }
}