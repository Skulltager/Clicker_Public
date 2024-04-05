using UnityEngine;
using UnityEngine.UI;

public class MiniMapDisplay : MonoBehaviour
{
    [SerializeField] private Transform cameraRotation;
    [SerializeField] private new Camera camera;
    [SerializeField] private RawImage cameraDisplay;
    [SerializeField] private PlayerController playerController;

    [SerializeField] private int maxScaleLevel;
    [SerializeField] private int minScaleLevel;
    [SerializeField] private int startScaleLevel;
    [SerializeField] private float baseScale;
    [SerializeField] private float scalePerLevel;
    [SerializeField] private float scalePerLevelFactor;

    private readonly ControlledEventVariable<MiniMapDisplay, int> currentScaleLevel;
    private RenderTexture renderTexture;

    private MiniMapDisplay()
    {
        currentScaleLevel = new ControlledEventVariable<MiniMapDisplay, int>(this, 0, Check_CurrentScaleLevel);
    }

    private int Check_CurrentScaleLevel(int value)
    {
        return Mathf.Clamp(value, minScaleLevel, maxScaleLevel);
    }

    private void Awake()
    {
        currentScaleLevel.value = startScaleLevel;
        currentScaleLevel.onValueChangeImmediate += OnValueChanged_CurrentScaleLevel;
    }

    private void Start()
    {
        int width = Mathf.CeilToInt(cameraDisplay.rectTransform.rect.width);
        int height = Mathf.CeilToInt(cameraDisplay.rectTransform.rect.height);
        renderTexture = new RenderTexture(width, height, 1, RenderTextureFormat.RGB111110Float);
        renderTexture.enableRandomWrite = true;
        renderTexture.Create();
        cameraDisplay.texture = renderTexture;

        camera.targetTexture = renderTexture;
    }

    private void OnEnable()
    {
        MainCameraEvents.onPreRender += OnEvent_PreRender;
    }

    private void OnEvent_PreRender(Camera camera)
    {
        Vector3 cameraPosition = camera.transform.position / MiniMapVisualizer.instance.ScaleDifference;
        cameraPosition.x += MiniMapVisualizer.instance.Offset.x;
        cameraPosition.z += MiniMapVisualizer.instance.Offset.y;
        cameraPosition.y = this.camera.transform.position.y;

        cameraRotation.rotation = playerController.transform.rotation;
        this.camera.transform.position = cameraPosition;
        this.camera.Render();
    }

    private void OnValueChanged_CurrentScaleLevel(int oldValue, int newValue)
    {
        float newScale = baseScale + (newValue * scalePerLevel) * Mathf.Pow(scalePerLevelFactor, newValue);
        camera.orthographicSize = newScale;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Minus))
            currentScaleLevel.value++;
        else if (Input.GetKeyDown(KeyCode.Equals))
            currentScaleLevel.value--;
    }

    private void OnDisable()
    {
        MainCameraEvents.onPreRender -= OnEvent_PreRender;
    }

    private void OnDestroy()
    {
        currentScaleLevel.onValueChange -= OnValueChanged_CurrentScaleLevel;
        renderTexture.Release();
    }
}
