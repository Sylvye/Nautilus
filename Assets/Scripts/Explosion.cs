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
        CameraController.main.ScreenShake(screenShakeIntensity);
    }
}
