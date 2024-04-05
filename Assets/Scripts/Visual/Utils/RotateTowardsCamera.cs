using System.Collections;
using UnityEngine;

public class RotateTowardsCamera : MonoBehaviour
{
    private void OnEnable()
    {
        MainCameraEvents.onPreRender += OnEvent_PreRender;
    }

    private void OnEvent_PreRender(Camera camera)
    {
        transform.rotation = Quaternion.Euler(0, camera.transform.rotation.eulerAngles.y, 0);
    }

    private void OnDisable()
    {
        MainCameraEvents.onPreRender -= OnEvent_PreRender;
    }
}