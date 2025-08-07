using System.Xml;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using static UnityEngine.UI.Image;

public class PlayerController : MonoBehaviour
{
    private bool boosting;
    private bool shifting;
    private Vessel v;
    private Vector2 moveInput;
    private PlayerInputActions inputActions;

    void Awake()
    {
        v = GetComponent<Vessel>();
        inputActions = new PlayerInputActions();
    }

    private void Start()
    {

    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += _ => moveInput = Vector2.zero;
        inputActions.Player.Jump.performed += _ => v.attacking = true;
        inputActions.Player.Jump.canceled += _ => v.attacking = false;
        inputActions.Player.Boost.performed += _ => boosting = true;
        inputActions.Player.Boost.canceled += _ => boosting = false;
        inputActions.Player.Nudge.performed += _ => shifting = true;
        inputActions.Player.Nudge.canceled += _ => shifting = false;
    }

    void OnDisable()
    {
        inputActions.Player.Disable();
    }

    void FixedUpdate()
    {
        if (moveInput != Vector2.zero)
        {
            v.Move(moveInput, (boosting ? 2 : 1) * (shifting ? 0.5f : 1));
            if (Mathf.Sin(AngleHelper.VectorToRadians(moveInput)) > 0.9f)
            {
                v.Stabilize();
            }
        }
        else
        {
            v.Stabilize();
        }
    }
}
