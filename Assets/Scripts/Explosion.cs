using UnityEngine;

public class Explosion : MonoBehaviour
{
    public ExplosionData data;
    public ParticleSystem ps;
    public LayerMask targets;
    public float shockwaveRadius;
    public float shockwavePower;
    public float shockwaveDamage;
    public float screenShakeIntensity;

    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
    }

    private void Start()
    {
        var psMain = ps.main;
        var psEmm = ps.emission;
        psMain.startSpeed = new ParticleSystem.MinMaxCurve(data.speedRange.x, data.speedRange.y);
        psEmm.SetBurst(0, new ParticleSystem.Burst(0, data.particles));

        if (TryGetComponent(out ParticleDamage pd))
        {
            pd.baseDamage = data.damage;
        }

        if (shockwaveRadius > 0)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, shockwaveRadius, targets);
            DrawDebugCircle(transform.position, shockwaveRadius, 30, Color.orange, 2);
            foreach (Collider2D hit in hits)
            {
                if (hit != null && hit.TryGetComponent(out Body body))
                {
                    Vector2 dir = (body.transform.position - transform.position);
                    body.rb.AddForce((shockwaveRadius - dir.magnitude) * shockwavePower * dir);
                    body.DealDamage(new Damage(shockwaveDamage, Damage.Type.Incendiary));
                }
            }
        }
        MainCameraController.main.ScreenShake(screenShakeIntensity);
    }

    private void DrawDebugCircle(Vector3 center, float radius, int segments, Color color, float time)
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
