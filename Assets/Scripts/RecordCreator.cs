using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class RecordCreator : MonoBehaviour
{
    public static RecordCreator main;
    private PlayerInputActions inputActions;
    public GameObject window;
    public GameObject initializer;
    public GameObject pointCreator;
    public GameObject dirCreator;
    public GameObject distCreator;
    public GameObject areaCreator;
    public GameObject activeMenu;
    public GameObject beacon;
    public Record previewRecord;

    public Slider colorSlider;
    public TMP_InputField labelField;
    public TMP_InputField xField;
    public TMP_InputField yField;
    public TMP_Dropdown typeDropdown;

    private Canvas canvas;
    private RectTransform windowRectT;

    private void Awake()
    {
        main = this;
        inputActions = new PlayerInputActions();
        canvas = GetComponentInParent<Canvas>().rootCanvas;
        windowRectT = window.GetComponent<RectTransform>();
        activeMenu = initializer;
    }

    private void OnEnable()
    {
        inputActions.UI.Enable();
        inputActions.UI.RightClick.canceled += _ => SummonWindowToMouse();
        inputActions.UI.Spacebar.performed += _ => SummonWindowToPlayer();
    }

    void OnDisable()
    {
        inputActions.UI.Disable();
    }

    private void SummonWindowToPlayer()
    {
        if (CameraManager.currentCamera == CameraManager.CameraType.map)
        {
            windowRectT.anchoredPosition = Vector2.one * 0.5f;
            window.SetActive(true);
            beacon.SetActive(true);

            Vector2 pos = PlayerController.main.transform.position;
            xField.text = pos.x * 0.1f + "";
            yField.text = pos.y * 0.1f + "";
            beacon.transform.position = new(pos.x, pos.y, -30);
            beacon.GetComponent<SpriteRenderer>().color = Color.HSVToRGB(colorSlider.value, 1, 1);
        }
    }

    private void SummonWindowToMouse()
    {
        if (CameraManager.currentCamera == CameraManager.CameraType.map)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Mouse.current.position.ReadValue(), null, out Vector2 localMousePos);
            windowRectT.anchoredPosition = localMousePos;
            window.SetActive(true);
            beacon.SetActive(true);


            Vector2 worldMousePos = MapCameraController.main.c.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            xField.text = worldMousePos.x * 0.1f + "";
            yField.text = worldMousePos.y * 0.1f + "";
            beacon.transform.position = new(worldMousePos.x, worldMousePos.y, -30);
            beacon.GetComponent<SpriteRenderer>().color = Color.HSVToRGB(colorSlider.value, 1, 1);
        }
    }

    public static void ResetFields(GameObject menu)
    {
        TMP_Dropdown[] dropdowns = menu.GetComponentsInChildren<TMP_Dropdown>();
        TMP_InputField[] inputFields = menu.GetComponentsInChildren<TMP_InputField>();
        Slider[] sliders = menu.GetComponentsInChildren<Slider>();

        foreach(TMP_Dropdown dropdown in dropdowns)
        {
            dropdown.value = 0;
            dropdown.RefreshShownValue();
        }

        foreach (TMP_InputField inputField in inputFields)
        {
            inputField.text = "";
            inputField.DeactivateInputField();
        }

        foreach (Slider slider in sliders)
        {
            slider.value = 0;
        }
    }

    public static Record.RecordType StringToType(string type)
    {
        return (Record.RecordType)Enum.Parse(typeof(Record.RecordType), type);
    }

    public static void ExitRecordCreator()
    {
        // clear values in the active field & initializer menu
        ResetFields(main.initializer);
        ResetFields(main.activeMenu);
        if (main.previewRecord != null)
            Destroy(main.previewRecord.gameObject);

        main.activeMenu.SetActive(false);
        main.initializer.SetActive(true);
        main.window.SetActive(false);
        main.beacon.SetActive(false);
    }
}