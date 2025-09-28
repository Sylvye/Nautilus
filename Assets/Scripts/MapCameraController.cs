using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class MapCameraController : MonoBehaviour
{
    public static MapCameraController main;
    public float zoomSpeed;
    public Vector2 zoomClamp;
    public static List<Record> records;
    public List<Record> recordPrefabs;
    private PlayerInputActions inputActions;
    public float zoomAmount = 1;
    public Camera c;
    private float startSize;
    private GameObject grid;
    private Vector3 gridOriginalScale;
    private Vector2 mDelta;
    private Vector2 lastMPos;
    private bool selected;

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
        inputActions.UI.Enable();
        inputActions.UI.ScrollWheel.performed += scrl => zoomAmount = Mathf.Clamp(zoomAmount + scrl.ReadValue<Vector2>().y * -zoomSpeed, zoomClamp.x, zoomClamp.y);
        inputActions.UI.Click.performed += _ => selected = !EventSystem.current.IsPointerOverGameObject();
        inputActions.UI.Click.canceled += _ => selected = false;
        inputActions.UI.Point.performed += look => mDelta = look.ReadValue<Vector2>();
        inputActions.UI.Point.canceled += _ => mDelta = Vector2.zero;
        inputActions.UI.Toggle_Map.performed += _ => CameraManager.ToggleCamera();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    private void Update()
    {
        switch (CameraManager.currentCamera)
        {
            case CameraManager.CameraType.main:
                transform.position = new Vector3(PlayerController.main.transform.position.x, PlayerController.main.transform.position.y, -100);
                break;
            case CameraManager.CameraType.map:
                c.orthographicSize = startSize * zoomAmount;
                grid.transform.localScale = gridOriginalScale * zoomAmount;

                RecordCreator.main.beacon.transform.localScale = Vector3.one * zoomAmount;

                if (selected)
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

    public static void AddRecord(Record r)
    {
        records.Add(r);
    }

    public static Record FindRecord(string label)
    {
        return records.Find(x=>x.label == label);
    }

    public static void DeleteRecord(Record r)
    {
        records.Remove(r);
        Destroy(r);
    }

    public static PointRecord CreatePointRecord(string label, Color color, Vector2 pos)
    {
        PointRecord posR = Instantiate(main.recordPrefabs.Find(x => x.GetRecordType() == Record.RecordType.Point).gameObject).GetComponent<PointRecord>();
        posR.SetPosition(pos);
        posR.SetColor(color);
        posR.SetLabel(label);
        return posR;
    }

    public static DirectionRecord CreateDirectionRecord(string label, Color color, Vector2 pos, float angle)
    {
        DirectionRecord dirR = Instantiate(main.recordPrefabs.Find(x => x.GetRecordType() == Record.RecordType.Direction).gameObject).GetComponent<DirectionRecord>();
        dirR.SetPosition(pos);
        dirR.SetColor(color);
        dirR.SetLabel(label);
        dirR.SetAngle(angle);
        return dirR;
    }

    public static DistanceRecord CreateDistanceRecord(string label, Color color, Vector2 pos, float radius)
    {
        DistanceRecord distR = Instantiate(main.recordPrefabs.Find(x => x.GetRecordType() == Record.RecordType.Distance).gameObject).GetComponent<DistanceRecord>();
        distR.SetPosition(pos);
        distR.SetColor(color);
        distR.SetLabel(label);
        distR.SetRadius(radius);
        return distR;
    }

    public static AreaRecord CreateAreaRecord(string label, Color color, Vector2 pos, float radius)
    {
        AreaRecord ar = Instantiate(main.recordPrefabs.Find(x => x.GetRecordType() == Record.RecordType.Area).gameObject).GetComponent<AreaRecord>();
        ar.SetPosition(pos);
        ar.SetColor(color);
        ar.SetLabel(label);
        ar.SetRadius(radius);
        return ar;
    }
}
