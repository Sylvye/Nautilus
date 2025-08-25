using UnityEngine;

public static class CameraManager
{
    public enum CameraType
    {
        main,
        map
    }

    private static float prevTimeScale;

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
            prevTimeScale = Time.timeScale;
            Time.timeScale = 0;
        }
        else
        {
            mainCamera.enabled = false;
            mapCamera.enabled = true;
            Time.timeScale = prevTimeScale;
        }
    }

    public static void ToggleCamera()
    {
        if (cameraType == CameraType.main)
        {
            mainCamera.enabled = false;
            mapCamera.enabled = true;
            cameraType = CameraType.map;
            prevTimeScale = Time.timeScale;
            Time.timeScale = 0;
        }
        else
        {
            mainCamera.enabled = true;
            mapCamera.enabled = false;
            cameraType = CameraType.main;
            Time.timeScale = prevTimeScale;
        }
    }
}
