using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public ExplosionData data;
    public ParticleSystem ps;
    public LayerMask targets;
    public LayerMask ignoreLayer;
    public float raycasts;
    public float radius;
    public float power;
    public float damage;
    public float screenShakeIntensity;

    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
    }

    private void Start()
    {
        gameObject.layer = ignoreLayer;
        var psMain = ps.main;
        var psEmm = ps.emission;
        psMain.startSpeed = new ParticleSystem.MinMaxCurve(data.speedRange.x, data.speedRange.y);
        psEmm.SetBurst(0, new ParticleSystem.Burst(0, data.particles));

        if (TryGetComponent(out ParticleDamage pd))
        {
            pd.baseDamage = data.damage;
        }

        if (radius > 0)
        {
            List<Collider2D> hits = new();

            for (int i=0; i<raycasts; i++)
            {
                float angle = i * 360 / raycasts;
                Vector2 dir = AngleHelper.DegreesToVector(angle);
                RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, radius, targets);
                if (hit.point != Vector2.zero)
                    Debug.DrawLine(transform.position, hit.point, Color.orange, 2);
                else
                    Debug.DrawLine(transform.position, transform.position + (Vector3)(dir * radius), Color.red, 2);
                if (hit.collider != null && hit.collider.TryGetComponent(out Body body))
                {
                    body.rb.AddForce((radius - dir.magnitude) * power * dir);
                    body.DealDamage(new Damage(damage, Damage.Type.Incendiary));
                }
            }
        }
        MainCameraController.main.ScreenShake(screenShakeIntensity, transform.position);
    }

    private void DrawDebugCircle(Vector3 center, float radius, int segments, Color color, float time)
    {
        float angleStep = 360f / segments;
        Vector3 previousPoint = center + new Vector3(Mathf.Cos(0), Mathf.Sin(0), 0) * radius;

        for (int i = 1; i <= segments; i++)
        {
            float angle = angleStep * i * Mathf.Deg2Rad;
            Vector3 nextPoint = center + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius;
            Debug.DrawLine(previousPoint, nextPoint, color, time);
            previousPoint = nextPoint;
        }
    }
}
