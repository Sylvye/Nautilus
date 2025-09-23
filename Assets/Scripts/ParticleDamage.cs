using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ParticleDamage : MonoBehaviour
{
    public float baseDamage;
    public Damage.Type type;
    public Body source;
    private ParticleSystem ps;

    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
        if (transform.root != null)
        {
            source = transform.root.GetComponent<Body>();
        }
    }

    void OnParticleCollision(GameObject other)
    {
        if (other.TryGetComponent(out Body body) && (source == null || body.transform.root.gameObject != source.gameObject))
        {
            List<ParticleCollisionEvent> collisionEvents = new();
            int eventCount = ps.GetCollisionEvents(other, collisionEvents);

            for (int i = 0; i < eventCount; i++)
            {
                float speed = collisionEvents[i].velocity.magnitude;
                float damage = baseDamage * speed * 0.02f;

                body.DealDamage(new Damage(damage, type));
            }
        }
    }
}
