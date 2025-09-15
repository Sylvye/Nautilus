using UnityEngine;

public class ForceApplier : MonoBehaviour
{
    public float radius;
    public bool invSqrLaw;
    public LayerMask layerMask;
    public float strength;
    public float damage;
    public Damage.Type damageType;
    private Collider2D col;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
    }

    private void Update()
    {
        DrawDebugCircle(transform.position, radius, 30, Color.yellow);
    }

    private void FixedUpdate()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius, layerMask);

        foreach (Collider2D hit in hits)
        {
            if (hit.TryGetComponent(out Rigidbody2D r))
            {
                Vector2 dir = transform.position - r.transform.position;
                float str = strength * (invSqrLaw ? 1/Mathf.Pow(dir.magnitude, 2) : 1);
                r.AddForce(dir * str, ForceMode2D.Force);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Body b))
        {
            b.DealDamage(new Damage(damage, damageType));
            DrawDebugCircle(b.transform.position, 0.5f, 15, Color.crimson);
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
