using System.Collections;
using UnityEngine;

public abstract class VesselComponent : MonoBehaviour
{
    public float maxHP;
    public float hp;
    public float respawnCooldown; // leave at 0 for non-respawning components
    protected Rigidbody2D vesselRB;
    protected Collider2D col;
    private SpriteRenderer sr;

    private void Awake()
    {
        vesselRB = GetComponentInParent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        sr = GetComponentInParent<SpriteRenderer>();
    }

    public virtual void Damage(float amount)
    {
        hp -= amount;
        if (hp < 0)
        {
            hp = 0;
            if (respawnCooldown > 0)
            {
                StartCoroutine(RespawnCoroutine());
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    private IEnumerator RespawnCoroutine()
    {
        sr.enabled = false;
        col.enabled = false;
        yield return new WaitForSeconds(respawnCooldown);
        sr.enabled = true;
        col.enabled = true;
    }
}
