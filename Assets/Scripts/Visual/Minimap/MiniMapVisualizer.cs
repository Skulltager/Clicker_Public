using System.Collections.Generic;
using UnityEngine;

public class MiniMapVisualizer : DataDrivenBehaviour<MapGenerator>
{
    public static MiniMapVisualizer instance { private set; get; }
    [SerializeField] private float scaleDifference;
    [SerializeField] private Vector2 offset;
    [SerializeField] private Transform miniMapChunksContainer;
    [SerializeField] private MiniMapChunkVisualizer miniMapChunkVisualPrefab;
    [SerializeField] private MiniMapMarkerManager miniMapMarkerManager;

    public float ScaleDifference => scaleDifference;
    public Vector2 Offset => offset;

    private readonly List<MiniMapChunkVisualizer> miniMapChunkVisuals;
    private readonly List<MiniMapChunkVisualizer> miniMapChunksRendering;

    public readonly EventVariable<MiniMapVisualizer, MinMaxVector2> minMaxVector;

    private MiniMapVisualizer()
    {
        miniMapChunkVisuals = new List<MiniMapChunkVisualizer>();
        miniMapChunksRendering = new List<MiniMapChunkVisualizer>();
        minMaxVector = new EventVariable<MiniMapVisualizer, MinMaxVector2>(this, default);
    }

    private void Awake()
    {
        instance = this;
    }

    protected override void OnValueChanged_Data(MapGenerator oldValue, MapGenerator newValue)
    {
        if (oldValue != null)
        {
            foreach (MiniMapChunkVisualizer mapChunkVisual in miniMapChunkVisuals)
            {
                GameObject.Destroy(mapChunkVisual.gameObject);
                mapChunkVisual.shouldRender.onValueChangeSource -= OnValueChanged_MiniChunkVisual_ShouldRender;
            }

            miniMapChunkVisuals.Clear();

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

    public void AddMiniMapMarker(Transform followTransform, Texture texture)
    {
        miniMapMarkerManager.AddMiniMapMarker(followTransform, texture);
    }

    public void RemoveMiniMapMarker(Transform followTransform)
    {
        miniMapMarkerManager.RemoveMiniMapMarker(followTransform);
    }

    public void UpdatePositions(Camera camera, float sizeMultiplier)
    {
        miniMapMarkerManager.UpdatePositions(camera, sizeMultiplier);
    }

    private void OnAdd_MapChunk(MapChunk item)
    {
        MiniMapChunkVisualizer instance = GameObject.Instantiate(miniMapChunkVisualPrefab, miniMapChunksContainer);
        instance.shouldRender.onValueChangeImmediateSource += OnValueChanged_MiniChunkVisual_ShouldRender;
        instance.transform.localPosition = new Vector3(item.xIndex * data.ChunkSize, item.yIndex * data.ChunkSize, 0);
        instance.data = item;
        miniMapChunkVisuals.Add(instance);
    }

    private void OnRemove_MapChunk(MapChunk item)
    {
        MiniMapChunkVisualizer instance = miniMapChunkVisuals.Find(i => i.data == item);
        miniMapChunkVisuals.Remove(instance);
        GameObject.Destroy(instance.gameObject);
    }

    private void OnValueChanged_MiniChunkVisual_ShouldRender(MiniMapChunkVisualizer source, bool oldValue, bool newValue)
    {
        if (oldValue)
        {
            miniMapChunksRendering.Remove(source);
            source.minMaxVector.onValueChange -= OnValueChanged_MiniChunkVisual;
        }

        if (newValue)
        {
            miniMapChunksRendering.Add(source);
            source.minMaxVector.onValueChange += OnValueChanged_MiniChunkVisual;
        }

        RecalculateBounds();
    }

    private void OnValueChanged_MiniChunkVisual(MinMaxVector2 oldValue, MinMaxVector2 newValue)
    {
        RecalculateBounds();
    }

    private void RecalculateBounds()
    {
        if (miniMapChunksRendering.Count == 0)
        {
            minMaxVector.value = MinMaxVector2.CreateEmpty();
            return;
        }

        MinMaxVector2 bounds = miniMapChunksRendering[0].minMaxVector.value;

        for(int i = 1; i < miniMapChunksRendering.Count; i++)
            bounds = MinMaxVector2.AssignHighestOffsets(bounds, miniMapChunksRendering[i].minMaxVector.value);

        bounds.xMin++;
        bounds.yMin++;
        minMaxVector.value = bounds;
    }
}
