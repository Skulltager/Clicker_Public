using System;
using System.Diagnostics;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class MainCameraEvents : MonoBehaviour
{
    public static event Action<Camera> onPreRender;

    private new Camera camera;

    private void Awake()
    {
        camera = GetComponent<Camera>();
        camera.depthTextureMode = camera.depthTextureMode | DepthTextureMode.Depth;
    }

    private void OnPreRender()
    {
        if (!Camera.main == camera)
            return;

        if (onPreRender != null)
            onPreRender(camera);
    }
}