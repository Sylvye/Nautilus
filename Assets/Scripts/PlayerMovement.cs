using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public float jump;

    public GameObject cell;


    private PlayerInputActions inputActions;
    private Vector2 moveInput;
    private Vector2 velocity;
    private bool facingRight;
    private bool grounded;
    private bool bumpedHead;
    private Rigidbody2D rb;
    private Collider2D col;
    private bool jumpRequested;
    private bool attackRequested;
    private float lastJump;
    private LayerMask jumpable;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        inputActions = new PlayerInputActions();
        col = GetComponent<Collider2D>();
        jumpable = LayerMask.GetMask("Terrain", "Cell", "Connector");
        facingRight = true;
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += _ => moveInput = Vector2.zero;
        inputActions.Player.Jump.performed += _ => jumpRequested = true;
        inputActions.Player.Attack.performed += _ => attackRequested = true;
    }

    void OnDisable()
    {
        inputActions.Player.Disable();
    }

    void FixedUpdate()
    {
        rb.AddForceX(moveInput.x * speed, ForceMode2D.Force);

        if (jumpRequested && IsGrounded() && Time.time-lastJump > 0.2f)
        {
            rb.AddForceY(jump, ForceMode2D.Impulse);
            lastJump = Time.time;
            jumpRequested = false;
        }

        if (attackRequested)
        {
            LaunchCell();
            attackRequested = false;
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.Raycast(transform.position, Vector2.down, col.bounds.extents.y + 0.2f, jumpable);
    }

    private void LaunchCell()
    {
        Vector2 target = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector2 dir = (target - (Vector2)transform.position).normalized;
        float power = 5;

        GameObject cellObj = Instantiate(cell, (Vector2)transform.position + dir, Quaternion.identity);
        Rigidbody2D cellRB = cellObj.GetComponent<Rigidbody2D>();
        cellRB.linearVelocity = dir * power;
    }
}
