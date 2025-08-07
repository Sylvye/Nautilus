using UnityEngine;
using static UnityEngine.InputSystem.Controls.AxisControl;

public class BoidController : MonoBehaviour
{
    public bool debug;

    public string id;
    public float speed;
    public float visionRadius;
    public LayerMask visionMask;
    [Header("Movement Constraints")]
    [Range(0, 180)] public float forwardAngleLimit;
    [Range(0, 180)] public float backwardAngleLimit;
    [Header("Boid constraints")]
    public float separation;
    public float alignment;
    public float cohesion;
    private Vessel v;

    private void Awake()
    {
        v = GetComponent<Vessel>();
    }

    private void FixedUpdate()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, visionRadius, visionMask);

        Vector2 separationForce = Vector2.zero;
        Vector2 avgCenter = Vector2.zero;
        Vector2 avgAlign = Vector2.zero;

        int nearby = 0;

        foreach (Collider2D hit in hits)
        {
            if (hit != null && hit.CompareTag("Enemy") && hit.TryGetComponent(out Vessel hitV) && hitV != v)
            {
                avgCenter += (Vector2)hitV.transform.position;
                avgAlign += (Vector2)hitV.transform.right;

                Vector2 offset = transform.position - hitV.transform.position;
                float distSqr = offset.sqrMagnitude;
                if (distSqr > 0.001f) // avoid div by 0
                    separationForce += offset / distSqr;

                nearby++;
            }
        }
        Debug.Log(nearby + " nearby enemies");

        avgCenter /= nearby;
        avgAlign /= nearby;


        Vector2 steerForce = Vector2.zero;

        //separation
        steerForce += separationForce * separation;

        //alignment
        if (nearby > 0)
        {
            avgAlign /= nearby;
            steerForce += avgAlign.normalized * alignment;
        }

        // cohesion
        steerForce += (avgCenter - (Vector2)transform.position) * cohesion;

        // debug
        if (debug)
        {
            DrawCircle(transform.position, visionRadius, 90, Color.yellow);
            DrawCircle(avgCenter, 0.25f, 15, Color.green);
            Debug.DrawLine(transform.position, avgCenter, Color.green);
            Debug.DrawLine(transform.position, transform.position + (Vector3)separationForce.normalized * 2, Color.hotPink);
            Debug.DrawLine(transform.position, transform.position + (Vector3)avgAlign.normalized, Color.purple);
        }

        Vector2 localSteer = Quaternion.Inverse(transform.rotation) * steerForce; // BLACK MAGIC

        if (localSteer != Vector2.zero)
        {
            float angleToForward = Vector2.SignedAngle(Vector2.up, localSteer);

            if (angleToForward >= -forwardAngleLimit && angleToForward <= forwardAngleLimit)
            {
                // within forward arc
                v.Move(localSteer);
            }
            else if (angleToForward >= 180f - backwardAngleLimit || angleToForward <= -180f + backwardAngleLimit)
            {
                // within rear arc
                v.Move(localSteer);
            }
            else
            {
                // outside allowed arc – clamp to nearest limit
                float clampedAngle;
                if (angleToForward > forwardAngleLimit && angleToForward < 180f - backwardAngleLimit)
                    clampedAngle = forwardAngleLimit;
                else if (angleToForward < -forwardAngleLimit && angleToForward > -180f + backwardAngleLimit)
                    clampedAngle = -forwardAngleLimit;
                else
                    clampedAngle = angleToForward > 0 ? 180f - backwardAngleLimit : -180f + backwardAngleLimit;

                Vector2 clampedLocalSteer = AngleHelper.DegreesToVector(clampedAngle) * localSteer.magnitude;
                v.Move(clampedLocalSteer);
            }

            v.Move(localSteer);
            if (Mathf.Sin(AngleHelper.VectorToRadians(localSteer)) > 0.9f)
            {
                v.Stabilize();
            }
        }
    }

    private void DrawCircle(Vector3 center, float radius, int segments, Color color)
    {
        float angleStep = 360f / segments;
        Vector3 previousPoint = center + new Vector3(Mathf.Cos(0), Mathf.Sin(0), 0) * radius;

        for (int i = 1; i <= segments; i++)
        {
            float angle = angleStep * i * Mathf.Deg2Rad;
            Vector3 nextPoint = center + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius;
            Debug.DrawLine(previousPoint, nextPoint, color);
            previousPoint = nextPoint;
        }
    }
}
