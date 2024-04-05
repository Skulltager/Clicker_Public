using UnityEngine;

public class MiniMapMarkerData
{
    public readonly Transform followTransform;
    public readonly Material material;

    public MiniMapMarkerData(Transform followTransform, Material material)
    {
        this.followTransform = followTransform;
        this.material = material;
    }
}

public class MiniMapMarker : DataDrivenBehaviour<MiniMapMarkerData>
{
    [SerializeField] private MeshRenderer iconMeshRenderer;
    [SerializeField] private float distanceFromCameraHeightOffset;
    [SerializeField] private float distanceFromCameraOffset;
    private float startPositionY;

    private void Start()
    {
        startPositionY = transform.position.y;
    }

    protected override void OnValueChanged_Data(MiniMapMarkerData oldValue, MiniMapMarkerData newValue)
    {
        if(newValue != null)
        {
            iconMeshRenderer.material = newValue.material;
        }
    }

    public void UpdatePosition(Camera camera, float sizeMultiplier)
    {
        Vector3 position = data.followTransform.position / MiniMapVisualizer.instance.ScaleDifference;
        position.x += MiniMapVisualizer.instance.Offset.x;
        position.z += MiniMapVisualizer.instance.Offset.y;
        
        Vector3 difference = camera.transform.position - position;
        difference.y = 0;
        float distance = Vector3.Dot(difference, camera.transform.up);

        position.y = startPositionY + distance * distanceFromCameraHeightOffset - difference.magnitude * distanceFromCameraOffset;
        transform.position = position;

        transform.rotation = camera.transform.rotation;
        transform.localScale = Vector3.one * camera.orthographicSize * sizeMultiplier;
    }
}