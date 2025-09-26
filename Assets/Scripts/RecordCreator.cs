using UnityEngine;
using UnityEngine.InputSystem;

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
    private Canvas canvas;
    private RectTransform rectT;

    private void Awake()
    {
        main = this;
        inputActions = new PlayerInputActions();
        canvas = GetComponentInParent<Canvas>();
        rectT = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        inputActions.UI.Enable();
        inputActions.UI.RightClick.canceled += _ => OnRightClick();
    }

    void OnDisable()
    {
        inputActions.UI.Disable();
    }

    private void OnRightClick()
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), Mouse.current.position.ReadValue(), null, out Vector2 localMPos);
        rectT.anchoredPosition = localMPos;
        window.SetActive(true);
    }
}
