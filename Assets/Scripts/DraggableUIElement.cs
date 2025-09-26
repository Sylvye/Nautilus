using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class DraggableUIElement : MonoBehaviour
{
    private Vector2 lastMPos;
    private PlayerInputActions inputActions;
    private Canvas canvas;
    private RectTransform rectT;
    private bool selected = false;

    private void Awake()
    {
        inputActions = new();
        canvas = GetComponentInParent<Canvas>();
        rectT = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        inputActions.UI.Enable();
        inputActions.UI.Click.performed += _ => selected = IsSelected();
        inputActions.UI.Click.canceled += _ => selected = false;
    }

    private void OnDisable()
    {
        inputActions.UI.Disable();
    }

    private void Update()
    {
        Vector2 mPos = Mouse.current.position.ReadValue();
        if (selected)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), mPos, null, out Vector2 localMPos);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), lastMPos, null, out Vector2 lastLocalMPos);
            rectT.anchoredPosition += localMPos - lastLocalMPos;
        }
        lastMPos = mPos;
    }

    private bool IsSelected()
    {
        return RectTransformUtility.RectangleContainsScreenPoint(rectT, Mouse.current.position.ReadValue());
    }
}
