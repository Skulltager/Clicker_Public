using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class CeilingRenderer : MonoBehaviour
{
    private ChunkRoomVisual chunkRoomVisual;
    private MeshRenderer meshRenderer;

    private void Awake()
    {
        chunkRoomVisual = GetComponentInParent<ChunkRoomVisual>();
        meshRenderer = GetComponent<MeshRenderer>();
        chunkRoomVisual.ceilingMaterial.onValueChangeImmediate += OnValueChanged_Material;
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
        chunkRoomVisual.ceilingMaterial.onValueChange -= OnValueChanged_Material;
    }
}