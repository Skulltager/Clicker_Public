using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class RoomDoorRenderer : MonoBehaviour
{
    private const string PROPERTY_ROOMCOLOR = "roomColor";
    private MeshRenderer meshRenderer;
    private DoorData roomData;

    private void Awake()
    {
        roomData = GetComponentInParent<DoorData>();
        meshRenderer = GetComponent<MeshRenderer>();
        roomData.fromChunkRoom.onValueChangeImmediate += OnValueChanged_RoomData;
    }

    private void OnValueChanged_RoomData(ChunkRoom oldValue, ChunkRoom newValue)
    {
        if (oldValue != null)
        {
            oldValue.roomColor.onValueChange -= OnValueChanged_RoomColor;
        }

        if (newValue != null)
        {
            newValue.roomColor.onValueChangeImmediate += OnValueChanged_RoomColor;
        }
    }

    private void OnValueChanged_RoomColor(float oldValue, float newValue)
    {
        meshRenderer.material.SetFloat(PROPERTY_ROOMCOLOR, newValue);
    }


    private void OnDestroy()
    {
        roomData.fromChunkRoom.onValueChangeImmediate -= OnValueChanged_RoomData;
    }
}