using System.Xml;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using static UnityEngine.UI.Image;

public class PlayerController : MonoBehaviour
{
    [Header("Motion")]
    private bool boosting;
    private bool shifting;

    [Header("Actions")]
    private bool attackRequested;

    [Header("Other")]
    public Collider2D bodyCol;

    private PlayerInputActions inputActions;
    private PlayerAbilityHandler abilityHandler;
    [SerializeField]private Vector2 moveInput;
    private Rigidbody2D rb;
    public LayerMask collidable;
    private Vessel v;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        v = GetComponent<Vessel>();
        abilityHandler = GetComponent<PlayerAbilityHandler>();
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
        inputActions.Player.Jump.performed += _ => boosting = true;
        inputActions.Player.Jump.canceled += _ => boosting = false;
        inputActions.Player.Attack.performed += _ => abilityHandler.Fire(); 
        inputActions.Player.Sprint.performed += _ => shifting = true;
        inputActions.Player.Sprint.canceled += _ => shifting = false;
        inputActions.Player.Next.performed += _ => abilityHandler.NextItem();
        inputActions.Player.Previous.performed += _ => abilityHandler.PrevItem();
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
        }
    }
}
