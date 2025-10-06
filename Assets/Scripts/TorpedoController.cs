using System.Runtime.InteropServices;
using UnityEngine;

public class TorpedoController : MonoBehaviour
{
    public Body target;
    public float rotateSpeed;
    private Vessel v;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        v = GetComponent<Vessel>();
    }

    private void FixedUpdate()
    {
        Vector2 dir = target.transform.position - transform.position;
        float targetAngle = AngleHelper.VectorToDegrees(dir);
        float facingAngle = transform.eulerAngles.z+90;
        float angleDiff = Mathf.DeltaAngle(facingAngle, targetAngle);
        rb.AddTorque(Mathf.Sign(angleDiff) * rotateSpeed, ForceMode2D.Force);
        v.Move(Vector2.up);
    }
}
