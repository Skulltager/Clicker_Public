using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class DoorRenderer : MonoBehaviour
{
    private const string PROPERTY_FROMROOMCOLOR = "fromRoomColor";
    private const string PROPERTY_TOROOMCOLOR = "toRoomColor";
    private MeshRenderer meshRenderer;
    private DoorData doorData;

    private void Awake()
    {
        doorData = GetComponentInParent<DoorData>();
        meshRenderer = GetComponent<MeshRenderer>();
        doorData.fromChunkRoom.onValueChangeImmediate += OnValueChanged_FromRoomData;
        doorData.toChunkRoom.onValueChangeImmediate += OnValueChanged_ToRoomData;
    }

    private void OnValueChanged_FromRoomData(ChunkRoom oldValue, ChunkRoom newValue)
    {
        if (oldValue != null)
        {
            oldValue.roomColor.onValueChange -= OnValueChanged_FromRoomColor;
        }

        if (newValue != null)
        {
            newValue.roomColor.onValueChangeImmediate += OnValueChanged_FromRoomColor;
        }
    }

    private void OnValueChanged_ToRoomData(ChunkRoom oldValue, ChunkRoom newValue)
    {
        if (oldValue != null)
        {
            oldValue.roomColor.onValueChange -= OnValueChanged_ToRoomColor;
        }

        if (newValue != null)
        {
            newValue.roomColor.onValueChangeImmediate += OnValueChanged_ToRoomColor;
        }
    }

    private void OnValueChanged_ToRoomColor(float oldValue, float newValue)
    {
        meshRenderer.material.SetFloat(PROPERTY_TOROOMCOLOR, newValue);
    }

    private void OnValueChanged_FromRoomColor(float oldValue, float newValue)
    {
        meshRenderer.material.SetFloat(PROPERTY_FROMROOMCOLOR, newValue);
    }

    private void OnDestroy()
    {
        doorData.fromChunkRoom.onValueChangeImmediate -= OnValueChanged_FromRoomData;
        doorData.toChunkRoom.onValueChangeImmediate -= OnValueChanged_ToRoomData;
    }
}