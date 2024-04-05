using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class WallRenderer : MonoBehaviour
{
    private ChunkRoomVisual chunkRoomVisual;
    private MeshRenderer meshRenderer;

    private void Awake()
    {
        chunkRoomVisual = GetComponentInParent<ChunkRoomVisual>();
        meshRenderer = GetComponent<MeshRenderer>();
        chunkRoomVisual.wallsMaterial.onValueChangeImmediate += OnValueChanged_Material;
    }

    private void OnValueChanged_Material(Material oldValue, Material newValue)
    {
        if (newValue != null)
        {
            meshRenderer.sharedMaterial = newValue;
        }
    }


    private void OnDestroy()
    {
        chunkRoomVisual.wallsMaterial.onValueChange -= OnValueChanged_Material;
    }
}