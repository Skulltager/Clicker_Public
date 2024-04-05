
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;

public class MapVisualizer : DataDrivenBehaviour<MapGenerator>
{
    [SerializeField] private Transform mapChunksContainer = default;
    [SerializeField] private MapChunkVisualizer mapChunkVisualPrefab = default;

    private readonly List<MapChunkVisualizer> mapChunkVisuals;

    private MapVisualizer()
    {
        mapChunkVisuals = new List<MapChunkVisualizer>();
    }

    protected override void OnValueChanged_Data(MapGenerator oldValue, MapGenerator newValue)
    {
        if (oldValue != null)
        {
            foreach (MapChunkVisualizer mapChunkVisual in mapChunkVisuals)
                GameObject.Destroy(mapChunkVisual.gameObject);

            mapChunkVisuals.Clear();

            oldValue.mapChunkList.onAdd -= OnAdd_MapChunk;
            oldValue.mapChunkList.onRemove -= OnRemove_MapChunk;
        }

        if (newValue != null)
        {
            newValue.mapChunkList.onAdd += OnAdd_MapChunk;
            newValue.mapChunkList.onRemove += OnRemove_MapChunk;
            foreach (MapChunk mapChunk in newValue.mapChunkList)
                OnAdd_MapChunk(mapChunk);
        }
    }

    private void OnAdd_MapChunk(MapChunk item)
    {
        MapChunkVisualizer instance = GameObject.Instantiate(mapChunkVisualPrefab, mapChunksContainer);
        instance.transform.localPosition = new Vector3(item.xIndex * data.ChunkSize, item.yIndex * data.ChunkSize, 0);
        instance.data = item;
        mapChunkVisuals.Add(instance);
    }

    private void OnRemove_MapChunk(MapChunk item)
    {
        MapChunkVisualizer instance = mapChunkVisuals.Find(i => i.data == item);
        mapChunkVisuals.Remove(instance);
        GameObject.Destroy(instance.gameObject);
    }
}
