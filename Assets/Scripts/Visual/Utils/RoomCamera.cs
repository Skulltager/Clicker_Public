using System.Diagnostics;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class RoomCamera : MonoBehaviour
{
    public const int DOOR_DEPTH_COUNT = 3;

    [SerializeField] private Camera roomCamera;
    [SerializeField] private Camera doorCamera;

    public readonly EventVariable<RoomCamera, RenderTexture> roomColorTexture;
    public readonly EventVariable<RoomCamera, RenderTexture> roomDepthTexture;
    public readonly RenderTexture[] doorColorTextures;
    public readonly RenderTexture[] doorDepthTextures;


    private RoomCamera()
    {
        roomColorTexture = new EventVariable<RoomCamera, RenderTexture>(this, null);
        roomDepthTexture = new EventVariable<RoomCamera, RenderTexture>(this, null);
        doorColorTextures = new RenderTexture[DOOR_DEPTH_COUNT];
        doorDepthTextures = new RenderTexture[DOOR_DEPTH_COUNT];
    }

    private void Awake()
    {
        CreateRenderTexture();
        roomCamera.depthTextureMode = DepthTextureMode.Depth;
        doorCamera.depthTextureMode = DepthTextureMode.Depth;
        Shader.SetGlobalInt("maxDoorDepth", DOOR_DEPTH_COUNT);
    }

    private void OnPreRender()
    {
        CheckRenderTexture();
        roomCamera.Render();

        for (int i = 0; i < DOOR_DEPTH_COUNT; i++)
        {
            Shader.SetGlobalInt("doorDepth", i);
            doorCamera.SetTargetBuffers(doorColorTextures[i].colorBuffer, doorDepthTextures[i].depthBuffer);
            doorCamera.Render();
            Shader.SetGlobalTexture("doorDepthTexture", doorDepthTextures[i]);
            Shader.SetGlobalTexture("doorColorTexture", doorColorTextures[i]);
        }
    }

    private void CheckRenderTexture()
    {
        if (roomColorTexture.value.width == Screen.width && roomColorTexture.value.height == Screen.height)
            return;

        roomDepthTexture.value.Release();
        roomColorTexture.value.Release();

        for (int i = 0; i < DOOR_DEPTH_COUNT; i++)
        {
            doorColorTextures[i].Release();
            doorDepthTextures[i].Release();
        }
        CreateRenderTexture();
    }

    private void CreateRenderTexture()
    {
        RenderTexture renderTexture = new RenderTexture(Screen.width, Screen.height, 0);
        renderTexture.filterMode = FilterMode.Point;
        renderTexture.wrapMode = TextureWrapMode.Clamp;
        renderTexture.Create();
        Shader.SetGlobalTexture("roomColorTexture", renderTexture);
        roomColorTexture.value = renderTexture;

        renderTexture = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.Depth);
        renderTexture.depthStencilFormat = GraphicsFormat.D24_UNorm;
        renderTexture.filterMode = FilterMode.Point;
        renderTexture.wrapMode = TextureWrapMode.Clamp;
        renderTexture.Create();
        Shader.SetGlobalTexture("roomDepthTexture", renderTexture);
        roomDepthTexture.value = renderTexture;

        roomCamera.SetTargetBuffers(roomColorTexture.value.colorBuffer, roomDepthTexture.value.depthBuffer);

        for(int i = 0; i < DOOR_DEPTH_COUNT; i++)
        {
            renderTexture = new RenderTexture(Screen.width, Screen.height, 0);
            renderTexture.filterMode = FilterMode.Point;
            renderTexture.wrapMode = TextureWrapMode.Clamp;
            renderTexture.Create();
            doorColorTextures[i] = renderTexture;
            Shader.SetGlobalTexture("doorColorTexture" + i, renderTexture);

            renderTexture = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.Depth);
            renderTexture.depthStencilFormat = GraphicsFormat.D24_UNorm;
            renderTexture.filterMode = FilterMode.Point;
            renderTexture.wrapMode = TextureWrapMode.Clamp;
            renderTexture.Create();
            doorDepthTextures[i] = renderTexture;
            Shader.SetGlobalTexture("doorDepthTexture" + i, renderTexture);
        }
    }

    private void OnDestroy()
    {
        roomColorTexture.value.Release();
        roomDepthTexture.value.Release();
        for (int i = 0; i < DOOR_DEPTH_COUNT; i++)
        {
            doorColorTextures[i].Release();
            doorDepthTextures[i].Release();
        }
    }
}