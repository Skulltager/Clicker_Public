using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class FloorRenderer : MonoBehaviour
{
    private ChunkRoomVisual chunkRoomVisual;
    private MeshRenderer meshRenderer;

    private void Awake()
    {
        chunkRoomVisual = GetComponentInParent<ChunkRoomVisual>();
        meshRenderer = GetComponent<MeshRenderer>();
        chunkRoomVisual.floorMaterial.onValueChangeImmediate += OnValueChanged_Material;
    }

    private void OnValueChanged_Material (Material oldValue, Material newValue)
    {
        if (newValue != null)
        {
            meshRenderer.sharedMaterial = newValue;
        }
    }


    private void OnDestroy()
    {
        chunkRoomVisual.floorMaterial.onValueChange -= OnValueChanged_Material;
    }
}