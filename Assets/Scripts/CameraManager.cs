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
    public static CameraType currentCamera = CameraType.main;

    public static void SwitchToCamera(CameraType ct)
    {
        currentCamera = ct;
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
            RecordCreator.main.window.SetActive(false);
        }
    }

    public static void ToggleCamera()
    {
        if (currentCamera == CameraType.main)
        {
            mainCamera.enabled = false;
            mapCamera.enabled = true;
            currentCamera = CameraType.map;
            prevTimeScale = Time.timeScale;
            Time.timeScale = 0;
        }
        else
        {
            mainCamera.enabled = true;
            mapCamera.enabled = false;
            currentCamera = CameraType.main;
            Time.timeScale = prevTimeScale;
        }
    }
}
