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
    private Camera c;
    private float startSize;
    private GameObject grid;
    private Vector3 gridOriginalScale;
    private bool mouseDown;
    private Vector2 mDelta;
    private Vector2 lastMPos;

    private void Awake()
    {
        main = this;
        inputActions = new PlayerInputActions();
        grid = GameObject.Find("Grid");
        gridOriginalScale = grid.transform.localScale;
        c = GetComponent<Camera>();
        startSize = c.orthographicSize;
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
                c.orthographicSize = startSize * zoomAmount;
                grid.transform.localScale = gridOriginalScale * zoomAmount;

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
                break;
        }
    }
}
