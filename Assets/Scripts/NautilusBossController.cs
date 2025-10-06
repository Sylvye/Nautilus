using UnityEngine;

public class NautilusBossController : Vessel
{
    public bool debug;
    public float rotateSpeed;
    public float safeRange;
    public float safeOffset;

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        Vector2 dir = PlayerController.main.transform.position - transform.position;
        float angleToPlayer = AngleHelper.VectorToDegrees(dir);
        float safeAngle = transform.eulerAngles.z + safeOffset - 180;
        float angleDiff = Mathf.DeltaAngle(safeAngle, angleToPlayer);

        //if (angleDiff > 180)
        //    angleDiff -= 360;

        if (Mathf.Abs(angleDiff) >= safeRange) // player is facing Nautilus's weak point
        {
            rb.AddTorque(Mathf.Sign(angleDiff) * rotateSpeed, ForceMode2D.Force);
            FireThrusters(0.2f);
        }
        else
        {
            rb.AddTorque(angleDiff / safeRange * rotateSpeed, ForceMode2D.Force);
            FireThrusters(1);
        }

        if (debug)
        {
            Debug.DrawLine(transform.position, transform.position + (Vector3)AngleHelper.DegreesToVector(safeAngle) * 10, Color.red);
            Debug.DrawLine(transform.position, transform.position + (Vector3)AngleHelper.DegreesToVector(angleToPlayer) * 10, Color.blue);
            Debug.DrawLine(transform.position, transform.position + (Vector3)AngleHelper.DegreesToVector(safeAngle + safeRange) * 10, Color.purple);
            Debug.DrawLine(transform.position, transform.position + (Vector3)AngleHelper.DegreesToVector(safeAngle - safeRange) * 10, Color.purple);
        }
    }


    private void FireThrusters(float r)
    {
        foreach (Thruster t in thrusters) // fire all thrusters
        {
            t.Activate(r);
        }
    }
}
