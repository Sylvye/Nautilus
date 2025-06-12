using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Locomotion")]
    public float groundAcceleration;
    public float airAcceleration;
    public float groundDeceleration;
    public float airDeceleration;
    public float speed;
    private bool facingRight;
    private bool shifting;
    // save size of body hitbox here to allow for switching between the two sizes
    [SerializeField] private bool grounded;

    [Header("Jump")]
    public float jump;
    public float fallMultiplier;
    private float originalGravityScale;
    private bool jumpRequested;
    private bool jumpReleaseRequested;
    [SerializeField ] private bool isJumping;
    private bool wasFalling;
    public float hangTime;
    private float hangTimeStart;
    public float coyoteTime;
    private float coyoteTimeStart;
    public float jumpBuffer;
    private float jumpBufferStart;



    [Header("Actions")]
    private bool attackRequested;

    [Header("Other")]
    public BoxCollider2D bodyCol;
    public BoxCollider2D feetCol;

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
        inputActions.Player.Jump.performed += _ => RequestJump();
        inputActions.Player.Jump.canceled += _ => jumpReleaseRequested = true;
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
        bool wasGrounded = grounded;
        grounded = IsGrounded();
        if (grounded && !wasGrounded) // just landed
        {
            isJumping = false;
            wasFalling = false;
            rb.gravityScale = originalGravityScale;
        }
        else if (!grounded && wasGrounded)
        {
            coyoteTimeStart = Time.time;
        }

        if (isJumping)
        {
            if (!wasFalling && rb.linearVelocity.y < 0)
            {
                wasFalling = true;
                hangTimeStart = Time.time;
            }

            if (hangTimeStart + hangTime >= Time.time)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
                rb.gravityScale = 0;
            }
            else
            {
                rb.gravityScale = originalGravityScale * fallMultiplier;
            }
        }

        if (facingRight && moveInput.x < 0)
        {
            Turn(false);
        }
        else if (!facingRight && moveInput.x > 0)
        {
            Turn(true);
        }

        float targetXVelocity = moveInput.x * speed;
        float acceleration = moveInput.x != 0 ? (grounded ? groundAcceleration : airAcceleration) : (grounded ? groundDeceleration : airDeceleration);
        float xVel = moveInput.x != 0 ? Mathf.Lerp(rb.linearVelocity.x, targetXVelocity, acceleration * Time.deltaTime) : Mathf.Lerp(rb.linearVelocity.x, 0, acceleration * Time.deltaTime);
        rb.linearVelocity = new Vector2(xVel, rb.linearVelocity.y);

        if (jumpRequested && !isJumping && (grounded || coyoteTimeStart + coyoteTime >= Time.time))
        {
            rb.AddForceY(jump, ForceMode2D.Impulse);
            isJumping = true;
            rb.gravityScale *= fallMultiplier;
            jumpRequested = false;
        }

        if (attackRequested)
        {
            abilityHandler.Fire();
            attackRequested = false;
        }

        if (shifting)
        {

        }
        else
        {
            bodyCol.size = new Vector2(bodyCol.size.x, bodyCol.size.y);
        }

        if (jumpRequested && jumpBufferStart + jumpBuffer < Time.time)
        {
            jumpRequested = false;
        }
    }

    private bool IsGrounded()
    {
        float detectionRayLength = 0.05f;
        Vector2 origin = new(transform.position.x, feetCol.bounds.min.y);
        Vector2 size = new(feetCol.bounds.size.x, detectionRayLength); // GROUND DETECTION RAY LENGTH
        return Physics2D.BoxCast(origin, size, 0, Vector2.down, detectionRayLength, collidable);
    }

    private void Turn(bool facingR)
    {
        facingRight = facingR;
        transform.Rotate(0, 180 * (facingR ? 1 : -1), 0);
    }

    private void RequestJump()
    {
        jumpRequested = true;
        jumpBufferStart = Time.time;
    }
}
