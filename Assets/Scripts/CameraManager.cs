using UnityEngine;

public static class CameraManager
{
    public enum CameraType
    {
        main,
        map
    }

    public static Camera mainCamera = MainCameraController.main.GetComponent<Camera>();
    public static Camera mapCamera = MapCameraController.main.GetComponent<Camera>();
    public static CameraType cameraType = CameraType.main;

    public static void SwitchToCamera(CameraType ct)
    {
        cameraType = ct;
        if (ct == CameraType.main)
        {
            mainCamera.enabled = true;
            mapCamera.enabled = false;
        }
        else
        {
            mainCamera.enabled = false;
            mapCamera.enabled = true;
        }
    }

    public static void ToggleCamera()
    {
        if (cameraType == CameraType.main)
        {
            mainCamera.enabled = false;
            mapCamera.enabled = true;
        }
        else
        {
            mainCamera.enabled = true;
            mapCamera.enabled = false;
        }
    }
}
