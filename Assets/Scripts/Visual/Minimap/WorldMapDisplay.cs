using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WorldMapDisplay : MonoBehaviour
{
    [SerializeField] private new Camera camera;
    [SerializeField] private RawImage cameraDisplay;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private float scaleDifference;
    [SerializeField] private Vector2 offset;
    [SerializeField] private GraphicRaycaster graphicsRaycaster;
    [SerializeField] private Canvas canvas;
    [SerializeField] private Vector2 uiOffset;
    [SerializeField] private Button closeButton;
    [SerializeField] private RoomContentsDisplay roomContentsDisplay;
    [SerializeField] private float padding;

    [SerializeField] private int maxScaleLevel;
    [SerializeField] private int minScaleLevel;
    [SerializeField] private int startScaleLevel;
    [SerializeField] private float baseScale;
    [SerializeField] private float scalePerLevel;
    [SerializeField] private float scalePerLevelFactor;
    [SerializeField] private float minDistanceForDrag;

    private readonly ControlledEventVariable<WorldMapDisplay, int> currentScaleLevel;
    private readonly EventVariable<WorldMapDisplay, MiniChunkRoomVisual> selectedMiniChunkRoom;
    private RenderTexture renderTexture;
    private MouseState mouseState;
    private Vector2 mouseClickPosition;
    private Vector3 dragStartingCameraPosition;
    private Vector2 dragStartedMousePosition;

    private WorldMapDisplay()
    {
        currentScaleLevel = new ControlledEventVariable<WorldMapDisplay, int>(this, 0, Check_CurrentScaleLevel);
        selectedMiniChunkRoom = new EventVariable<WorldMapDisplay, MiniChunkRoomVisual>(this, null);
    }

    private int Check_CurrentScaleLevel(int value)
    {
        return Mathf.Clamp(value, minScaleLevel, maxScaleLevel);
    }

    private void Awake()
    {
        int width = Mathf.CeilToInt(cameraDisplay.rectTransform.rect.width);
        int height = Mathf.CeilToInt(cameraDisplay.rectTransform.rect.height);
        renderTexture = new RenderTexture(width, height, 1, RenderTextureFormat.RGB111110Float);
        renderTexture.enableRandomWrite = true;
        renderTexture.Create();
        cameraDisplay.texture = renderTexture;

        camera.targetTexture = renderTexture;

        currentScaleLevel.value = startScaleLevel;
        currentScaleLevel.onValueChangeImmediate += OnValueChanged_CurrentScale;
        closeButton.onClick.AddListener(OnPress_CloseButton);
    }

    private void OnValueChanged_SelectedMiniChunkRoom(MiniChunkRoomVisual oldValue, MiniChunkRoomVisual newValue)
    {
        if (newValue != null)
        {
            Shader.SetGlobalInt("selectedRoom", newValue.data.roomIndex);
            roomContentsDisplay.data = newValue.data;
        }
        else
        {
            Shader.SetGlobalInt("selectedRoom", 0);
            roomContentsDisplay.data = null;
        }
    }

    private void OnValueChanged_CurrentScale(int oldValue, int newValue)
    {
        float oldSize = camera.orthographicSize;

        camera.orthographicSize = baseScale + (newValue * scalePerLevel) * Mathf.Pow(scalePerLevelFactor, newValue);

        float sizeFactor = (oldSize / camera.orthographicSize) - 1;

        Vector2 canvasSize = new Vector2(Screen.width, Screen.height) / canvas.scaleFactor;
        Rect corners = RectTransformUtility.PixelAdjustRect(cameraDisplay.rectTransform, canvas);

        Vector2 mousePosition = Input.mousePosition;
        mousePosition /= canvas.scaleFactor;
        mousePosition -= canvasSize / 2;
        mousePosition -= corners.min + uiOffset;
        mousePosition -= corners.size / 2;

        mousePosition /= corners.size.y / camera.orthographicSize / 2;
        mousePosition *= sizeFactor;
        camera.transform.position += new Vector3(mousePosition.x, 0, mousePosition.y);
        AdjustToScreenBounds();
    }

    private void OnEnable()
    {
        MainCameraEvents.onPreRender += OnEvent_PreRender;
        CursorController.instance.cursorVisibleCount.value++;
        mouseState = MouseState.None;
        selectedMiniChunkRoom.onValueChangeImmediate += OnValueChanged_SelectedMiniChunkRoom;
        AdjustToScreenBounds();
    }

    private void OnEvent_PreRender(Camera camera)
    {
        this.camera.Render();
    }

    private void OnPress_CloseButton()
    {
        IngameScreenManager.instance.ShowScreen_Hud();
    }

    private void Update()
    {
        Update_Zoom();

        if (mouseState == MouseState.None)
            HandleMouseState_None();

        if (mouseState == MouseState.Click)
            HandleMouseState_Click();

        if (mouseState == MouseState.Drag)
            HandleMouseState_Drag();

        camera.Render();
    }

    private void HandleMouseState_None()
    {
        if (!Input.GetMouseButtonDown(0))
            return;

        PointerEventData mouseLocation = new PointerEventData(EventSystem.current);
        mouseLocation.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        graphicsRaycaster.Raycast(mouseLocation, results);

        if (!results.Any(i => i.gameObject == cameraDisplay.gameObject))
            return;

        mouseClickPosition = Input.mousePosition;
        mouseState = MouseState.Click;
    }

    private void HandleMouseState_Click()
    {
        if (Input.GetMouseButtonUp(0))
        {
            TrySelectRoom();
            mouseState = MouseState.None;
            return;
        }

        Vector2 currentMousePosition = Input.mousePosition;
        Vector2 distance = currentMousePosition - mouseClickPosition;

        float distanceMag = distance.x * distance.x + distance.y * distance.y;
        float minDistanceMag = minDistanceForDrag * minDistanceForDrag;

        if (distanceMag < minDistanceMag)
            return;

        dragStartingCameraPosition = camera.transform.position;
        dragStartedMousePosition = mouseClickPosition;
        mouseState = MouseState.Drag;
    }

    private void AdjustToScreenBounds()
    {
        MinMaxVector2 screenBounds = MiniMapVisualizer.instance.minMaxVector.value;

        Vector3 cameraPosition = camera.transform.position;

        float aspectRatio = (float) renderTexture.width / renderTexture.height;

        float screenSizeY = camera.orthographicSize;
        float screenSizeX = screenSizeY * aspectRatio;

        screenSizeX -= padding;
        screenSizeY -= padding;

        float xPadding = Mathf.Max(screenSizeX - screenBounds.xSize, - screenSizeX);
        float yPadding = Mathf.Max(screenSizeY - screenBounds.ySize, - screenSizeY);

        float minX = screenBounds.xMin - xPadding;
        float maxX = screenBounds.xMax + xPadding;
        float minY = screenBounds.yMin - yPadding;
        float maxY = screenBounds.yMax + yPadding;

        cameraPosition.x = Mathf.Clamp(cameraPosition.x, minX, maxX);
        cameraPosition.z = Mathf.Clamp(cameraPosition.z, minY, maxY);

        camera.transform.position = cameraPosition;
    }

    private void TrySelectRoom()
    {
        Rect corners = RectTransformUtility.PixelAdjustRect(cameraDisplay.rectTransform, canvas);
        Vector2 canvasSize = new Vector2(Screen.width, Screen.height) / canvas.scaleFactor;
        Vector2 mousePosition = Input.mousePosition;
        mousePosition /= canvas.scaleFactor;
        mousePosition -= canvasSize / 2;
        mousePosition -= corners.min + uiOffset;
        mousePosition -= corners.size / 2;
        mousePosition /= corners.size;

        Vector3 centerRayPosition = camera.transform.position;
        centerRayPosition += new Vector3(mousePosition.x, 0, mousePosition.y) * camera.orthographicSize * 2;
        Ray ray = new Ray(centerRayPosition, Vector3.down);
        int layerMask = 1 << LayerMask.NameToLayer("Minimap");
        if (!Physics.Raycast(ray, out RaycastHit hit, 10, layerMask))
            return;

        if (!hit.collider.gameObject.TryGetComponent(out MiniChunkRoomVisual miniChunkRoomVisual))
            return;

        selectedMiniChunkRoom.value = miniChunkRoomVisual;
    }

    private void HandleMouseState_Drag()
    {
        if (Input.GetMouseButtonUp(0))
        {
            mouseState = MouseState.None;
            return;
        }

        Vector2 mousePosition = Input.mousePosition;
        Vector2 movedDistance = dragStartedMousePosition - mousePosition;
        movedDistance /= renderTexture.height / camera.orthographicSize / 2 * canvas.scaleFactor;

        Vector3 targetPosition = camera.transform.position;
        targetPosition.x = dragStartingCameraPosition.x + movedDistance.x;
        targetPosition.z = dragStartingCameraPosition.z + movedDistance.y;

        camera.transform.position = targetPosition;
        AdjustToScreenBounds();
    }

    private void Update_Zoom()
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        graphicsRaycaster.Raycast(pointerEventData, results);
        
        if (!results.Any(i => i.gameObject == cameraDisplay.gameObject))
            return;

        if (Input.mouseScrollDelta.y < 0)
            currentScaleLevel.value++;
        else if (Input.mouseScrollDelta.y > 0)
            currentScaleLevel.value--;
    }

    private void OnDisable()
    {
        MainCameraEvents.onPreRender -= OnEvent_PreRender;
        CursorController.instance.cursorVisibleCount.value--;
        selectedMiniChunkRoom.onValueChangeImmediate -= OnValueChanged_SelectedMiniChunkRoom;
    }

    private void OnDestroy()
    {
        closeButton.onClick.RemoveListener(OnPress_CloseButton);
        currentScaleLevel.onValueChange -= OnValueChanged_CurrentScale;
    }
}
