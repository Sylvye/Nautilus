using UnityEngine;

public class Shield : VesselComponent
{
    public float angularVelocity;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        rb.angularDamping = 0;
        rb.angularVelocity = angularVelocity;
    }
}
