using UnityEngine;
using UnityEngine.Rendering;
using static UnityEngine.InputSystem.Controls.AxisControl;

public class BoidController : MonoBehaviour
{
    public bool debug;
    [SerializeField]private float angleToForward;
    
    public string id;
    public float speed;
    public float visionRadius;
    public LayerMask visionMask;
    public bool tracking;
    public Vector2 destination;
    [Header("Movement Constraints")]
    [Range(0, 360)] public float angleLimit;
    [Header("Boid constraints")]
    public float separationForceMult;
    public float alignmentForceMult;
    public float cohesionForceMult;
    public float trackingForceMult;
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
            if (hit != null)
            {
                if (hit.CompareTag("Enemy") && hit.TryGetComponent(out Vessel hitV) && hitV != v)
                {
                    avgCenter += (Vector2)hitV.transform.position;
                    avgAlign += (Vector2)hitV.transform.up;

                    Vector2 offset = transform.position - hitV.transform.position;
                    float distSqr = offset.sqrMagnitude;
                    if (distSqr > 0.001f) // avoid div by 0
                        separationForce += offset / distSqr;

                    nearby++;
                }
                else if (hit.CompareTag("Debris")) // still avoid space debris but dont try to orient with 'em
                {
                    Vector2 offset = transform.position - hit.transform.position;
                    float distSqr = offset.sqrMagnitude;
                    if (distSqr > 0.001f) // avoid div by 0
                        separationForce += offset / distSqr * 2;
                }
            }
        }

        Vector2 steerForce = Vector2.zero;

        // separation
        steerForce += separationForce * separationForceMult;

        if (nearby > 0)
        {
            // averaging
            avgAlign /= nearby;
            avgCenter /= nearby;

            // alignment
            avgAlign /= nearby;
            steerForce += avgAlign.normalized * alignmentForceMult;

            // cohesion
            steerForce += (avgCenter - (Vector2)transform.position) * cohesionForceMult;
        }

        // tracking
        if (tracking)
            steerForce += (destination - (Vector2)transform.position).normalized * trackingForceMult;

        // debug
        if (debug)
        {
            if (nearby > 0)
            {
                DrawDebugCircle(avgCenter, 0.15f, 15, Color.green);
                Debug.DrawLine(transform.position, avgCenter, Color.green);
                Debug.DrawLine(transform.position, transform.position + (Vector3)avgAlign.normalized, Color.purple);
                Debug.DrawLine(transform.position, transform.position + (Vector3)separationForce.normalized * 2, Color.hotPink);
                DrawDebugCircle(transform.position, visionRadius, 90, Color.greenYellow);
            }
            else
            {
                DrawDebugCircle(transform.position, visionRadius, 90, Color.yellow);
            }
            if (tracking)
            {
                Debug.DrawLine(transform.position, destination, Color.cyan);
                DrawDebugCircle(destination, 0.3f, 30, Color.cyan);
            }
            if (angleLimit < 360)
            {
                Debug.DrawLine(transform.position, transform.position + (Vector3)AngleHelper.DegreesToVector(transform.eulerAngles.z - angleLimit + 90) * 2, Color.white);
                Debug.DrawLine(transform.position, transform.position + (Vector3)AngleHelper.DegreesToVector(transform.eulerAngles.z + angleLimit + 90) * 2, Color.white);
            }
        }

        Vector2 localSteer = Quaternion.Inverse(transform.rotation) * steerForce; // Converts world vector into relative local vector

        if (localSteer != Vector2.zero)
        {
            angleToForward = Vector2.SignedAngle(Vector2.up, localSteer);
            float magnitude = localSteer.magnitude;

            Debug.DrawLine(transform.position, transform.position + (Vector3)localSteer * magnitude * speed * 2, Color.blue);

            if (angleToForward <= angleLimit) // within forward arc
            {
                v.Move(localSteer, magnitude * speed);
            }
            else // clamp bad angles to nearest limit
            {
                float clampedAngle = Mathf.Clamp(Vector2.SignedAngle(localSteer, Vector2.right), -angleLimit, angleLimit);

                Vector2 clampedLocalSteer = Quaternion.Inverse(transform.rotation) * AngleHelper.DegreesToVector(clampedAngle) * magnitude;
                Debug.DrawLine(transform.position, transform.position + (Vector3)clampedLocalSteer * magnitude * speed * 2, Color.red);
                v.Move(clampedLocalSteer, magnitude * speed);
            }

            if (Mathf.Sin(AngleHelper.VectorToRadians(localSteer)) > 0.9f)
            {
                v.Stabilize();
            }
        }
    }

    private void DrawDebugCircle(Vector3 center, float radius, int segments, Color color)
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
