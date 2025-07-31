using UnityEngine;

public class Shield : VesselComponent
{
    public float angularVelocity;

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        rb.angularDamping = 0;
        rb.angularVelocity = angularVelocity;
    }
}
