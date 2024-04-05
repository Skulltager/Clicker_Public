using UnityEngine;

[RequireComponent(typeof(Camera))]

public class MiniMapCamera : MonoBehaviour
{
    [SerializeField] private float sizeMultiplier;
    private new Camera camera;

    private void Awake()
    {
        camera = GetComponent<Camera>();
    }

    private void OnPreRender()
    {
        MiniMapVisualizer.instance.UpdatePositions(camera, sizeMultiplier);
    }
}