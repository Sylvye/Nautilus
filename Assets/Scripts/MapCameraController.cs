using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class MapCameraController : MonoBehaviour
{
    public static MapCameraController main;
    public float zoomSpeed;
    private PlayerInputActions inputActions;
    private float zoomAmount = 1;
    private PixelPerfectCamera ppc;
    private Vector2 refRes;
    private GameObject grid;
    private Vector3 gridOriginalScale;

    private void Awake()
    {
        main = this;
        ppc = GetComponent<PixelPerfectCamera>();
        inputActions = new PlayerInputActions();
        refRes = new(ppc.refResolutionX, ppc.refResolutionY);
        grid = GameObject.Find("Grid");
        gridOriginalScale = grid.transform.localScale;
    }

    private void OnEnable()
    {
        inputActions.Enable();
        inputActions.UI.ScrollWheel.performed += scrl => zoomAmount += scrl.ReadValue<Vector2>().y * zoomSpeed;
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    private void Update()
    {
        switch (CameraManager.cameraType)
        {
            case CameraManager.CameraType.main:
                transform.position = new Vector3(PlayerController.main.transform.position.x, PlayerController.main.transform.position.y, -100);
                break;
            case CameraManager.CameraType.map:
                ppc.refResolutionX = (int)(refRes.x * zoomAmount);
                ppc.refResolutionY = (int)(refRes.y * zoomAmount);
                grid.transform.localScale = gridOriginalScale * zoomAmount;
                // TEMP: update grid scale value here
                Debug.Log(zoomAmount);
                break;
        }
    }
}
