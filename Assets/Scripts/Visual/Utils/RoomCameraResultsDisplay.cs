using UnityEngine;
using UnityEngine.UI;

public class RoomCameraResultsDisplay : MonoBehaviour
{
    [SerializeField] private RawImage roomColorTexture;
    [SerializeField] private RawImage roomDepthTexture;
    [SerializeField] private RawImage doorColorTexture;
    [SerializeField] private RawImage doorDepthTexture;
    [SerializeField] private RoomCamera roomCamera;

    private void Update()
    {
        roomColorTexture.texture = roomCamera.roomColorTexture.value;
        doorColorTexture.texture = roomCamera.doorColorTextures[0];
    }
}