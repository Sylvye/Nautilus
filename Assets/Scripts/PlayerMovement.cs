using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Locomotion")]
    public float groundAcceleration;
    public float airAcceleration;
    public float downPush;
    public float speed;
    public float sprintSpeed;
    private bool facingRight;
    private bool sprinting;
    [SerializeField] private bool grounded;

    [Header("Jump")]
    public float jump;
    public float fallMultiplier;
    private float originalGravityScale;
    private bool jumpRequested;
    private bool jumpReleaseRequested;
    private float lastJump;
    private bool isJumping;


    [Header("Actions")]
    private bool attackRequested;

    [Header("Other")]
    public Collider2D bodyCol;
    public Collider2D feetCol;

    private PlayerInputActions inputActions;
    private PlayerAbilityHandler abilityHandler;
    private Vector2 moveInput;
    private Vector2 velocity;
    private Rigidbody2D rb;
    private LayerMask collidable;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        abilityHandler = GetComponent<PlayerAbilityHandler>();
        inputActions = new PlayerInputActions();
        collidable = LayerMask.GetMask("Terrain", "Cell", "Connector");
        facingRight = true;
    }

    private void Start()
    {
        originalGravityScale = rb.gravityScale;
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += _ => moveInput = Vector2.zero;
        inputActions.Player.Jump.performed += _ => jumpRequested = true;
        inputActions.Player.Jump.canceled += _ => jumpReleaseRequested = true;
        inputActions.Player.Attack.performed += _ => abilityHandler.Fire(); 
        inputActions.Player.Sprint.performed += _ => sprinting = true;
        inputActions.Player.Sprint.canceled += _ => sprinting = false;
        inputActions.Player.Next.performed += _ => abilityHandler.NextItem();
        inputActions.Player.Previous.performed += _ => abilityHandler.PrevItem();
    }

    void OnDisable()
    {
        inputActions.Player.Disable();
    }

    void FixedUpdate()
    {
        bool wasGrounded = grounded;
        grounded = IsGrounded();
        if (grounded && !wasGrounded) // just landed
        {
            isJumping = false;
            rb.gravityScale = originalGravityScale;
        }

        if (facingRight && moveInput.x < 0)
        {
            Turn(false);
        }
        else if (!facingRight && moveInput.x > 0)
        {
            Turn(true);
        }

        float targetXVelocity = moveInput.x * (sprinting ? sprintSpeed : speed);
        float xVel = Mathf.Lerp(rb.linearVelocity.x, targetXVelocity, grounded ? groundAcceleration : airAcceleration);

        rb.AddForceX(xVel, ForceMode2D.Force);
        if (moveInput.y < 0)
            rb.AddForceY(moveInput.y * downPush, ForceMode2D.Force);

        if (jumpRequested && grounded && Time.time - lastJump > 0.2f)
        {
            rb.AddForceY(jump, ForceMode2D.Impulse);
            lastJump = Time.time;
            isJumping = true;
            rb.gravityScale *= fallMultiplier;
            jumpRequested = false;
        }

        if (attackRequested)
        {
            abilityHandler.Fire();
            attackRequested = false;
        }
    }

    private bool IsGrounded()
    {
        float detectionRayLength = 0.2f;
        Vector2 origin = new(transform.position.x, feetCol.bounds.min.y);
        Vector2 size = new(feetCol.bounds.size.x, detectionRayLength); // GROUND DETECTION RAY LENGTH
        return Physics2D.BoxCast(origin, size, 0, Vector2.down, detectionRayLength, collidable);
    }

    private void Turn(bool facingR)
    {
        facingRight = facingR;
        transform.Rotate(0, 180 * (facingR ? 1 : -1), 0);
    }
}
