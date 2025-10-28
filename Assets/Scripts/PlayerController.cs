using System.Xml;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using static UnityEngine.UI.Image;

public class PlayerController : MonoBehaviour
{
    private bool attacking;
    private bool boosting;
    private bool shifting;
    private Vector2 aimDir;
    private Vessel v;
    private Vector2 moveInput;
    private PlayerInputActions inputActions;

    private VolumeProfile globalVolumeProfile;
    private ChromaticAberration chromaticAberration;

    public static PlayerController main;

    void Awake()
    {
        main = this;
        v = GetComponent<Vessel>();
        inputActions = new PlayerInputActions();
        globalVolumeProfile = GameObject.Find("Global Volume").GetComponent<Volume>().profile;
        globalVolumeProfile.TryGet(out ChromaticAberration ca);
        chromaticAberration = ca;
    }

    private void Start()
    {

    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += _ => moveInput = Vector2.zero;
        inputActions.Player.Attack.performed += _ => attacking = true;
        inputActions.Player.Attack.canceled += _ => attacking = false;
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
        // Movement
        v.Move(moveInput, (boosting ? 2 : 1) * (shifting ? 0.5f : 1));
        if (moveInput != Vector2.zero)
        {
            if (Mathf.Sin(AngleHelper.VectorToRadians(moveInput)) > 0.9f)
            {
                v.Stabilize();
            }
        }
        else
        {
            v.Stabilize();
        }

        if (attacking)
        {
            foreach (Cannon c in v.cannons)
            {
                if (c.CanFire())
                {
                    c.Activate(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()), gameObject);
                }
            }
        }
    }

    private void Update()
    {
        chromaticAberration.intensity.value = Mathf.Lerp(0.1f, 1, 1 - v.hp / v.maxHP);
    }
}
