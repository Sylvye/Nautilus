using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ParticleDamage : MonoBehaviour
{
    public float baseDamage;
    public Damage.Type type;
    private ParticleSystem ps;

    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
    }

    void OnParticleCollision(GameObject other)
    {
        if (other.TryGetComponent(out Body body))
        {
            List<ParticleCollisionEvent> collisionEvents = new();
            int eventCount = ps.GetCollisionEvents(other, collisionEvents);

            for (int i = 0; i < eventCount; i++)
            {
                float speed = collisionEvents[i].velocity.magnitude;
                float damage = baseDamage * speed * 0.01f;

                body.DealDamage(new Damage(damage, type));
            }
        }
    }
}
