using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class SkyboxContentRenderer : MonoBehaviour
{
    private ChunkRoomVisual chunkRoomVisual;
    private MeshRenderer meshRenderer;

    private void Awake()
    {
        chunkRoomVisual = GetComponentInParent<ChunkRoomVisual>();
        meshRenderer = GetComponent<MeshRenderer>();
        chunkRoomVisual.data.roomColor.onValueChangeImmediate += OnValueChanged_RoomColor;
    }

    private void OnValueChanged_RoomColor(float oldValue, float newValue)
    {
        meshRenderer.material.SetFloat("roomColor", newValue);
    }


    private void OnDestroy()
    {
        chunkRoomVisual.data.roomColor.onValueChange -= OnValueChanged_RoomColor;
    }
}