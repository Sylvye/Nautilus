using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
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
    private bool mouseDown;
    private Camera c;
    private Vector2 mDelta;
    private Vector2 lastMPos;

    private void Awake()
    {
        main = this;
        ppc = GetComponent<PixelPerfectCamera>();
        inputActions = new PlayerInputActions();
        refRes = new(ppc.refResolutionX, ppc.refResolutionY);
        grid = GameObject.Find("Grid");
        gridOriginalScale = grid.transform.localScale;
        c = GetComponent<Camera>();
    }

    private void OnEnable()
    {
        inputActions.Enable();
        inputActions.UI.ScrollWheel.performed += scrl => zoomAmount = Mathf.Clamp(zoomAmount + scrl.ReadValue<Vector2>().y * -zoomSpeed, 1, 10);
        inputActions.Player.Attack.performed += _ => mouseDown = true;
        inputActions.Player.Attack.canceled += _ => mouseDown = false;
        inputActions.Player.Look.performed += look => mDelta = look.ReadValue<Vector2>();
        inputActions.Player.Look.canceled += _ => mDelta = Vector2.zero;
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
                ppc.refResolutionX = (int)(refRes.x * (int)zoomAmount);
                ppc.refResolutionY = (int)(refRes.y * (int)zoomAmount);
                grid.transform.localScale = gridOriginalScale * (int)zoomAmount;

                if (mouseDown)
                {
                    Vector2 mPos = Mouse.current.position.ReadValue();
                    Vector2 WMPos = c.ScreenToWorldPoint(mPos);
                    Vector2 lastWMPos = c.ScreenToWorldPoint(lastMPos);

                    transform.position += (Vector3)(lastWMPos - WMPos);

                    lastMPos = mPos;
                }
                else
                {
                    lastMPos = Mouse.current.position.ReadValue();
                }

                    // TEMP: update grid scale value here
                    Debug.Log(zoomAmount);
                break;
        }
    }
}
