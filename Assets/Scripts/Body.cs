using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Body : MonoBehaviour
{
    public float maxHP;
    public float hp;
    public float regeneration;
    public float collisionDamageMult;
    public GameObject deathFX;
    public List<Resistance> resistances;
    [NonSerialized] public Collider2D col;
    [NonSerialized] public Rigidbody2D rb;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = rb.GetComponent<Collider2D>();
    }

    protected virtual void FixedUpdate()
    {
        if (hp < maxHP)
        {
            hp += regeneration;
            if (hp > maxHP)
                hp = maxHP;
        }
    }

    /// <summary>
    /// Returns true if death occurs
    /// </summary>
    /// <param name="amount"></param>
    /// <returns></returns>
    public bool DealDamage(Damage amount)
    {
        if (hp > 0)
        {
            float amt = amount.Evaluate(resistances);
            hp -= amt;
            if (hp <= 0)
            {
                hp = 0;
                return OnDeath();
            }
        }
        return false;
    }

    /// <summary>
    /// Returns whether or not the death is confirmed
    /// </summary>
    /// <returns></returns>
    public virtual bool OnDeath()
    {
        SpawnDeathFX();
        Destroy(gameObject);
        KillChildren();
        return true;
    }

    protected void KillChildren()
    {
        foreach (Transform t in transform)
        {
            if (t.TryGetComponent(out Body b))
            {
                b.OnDeath();
                Debug.Log(b.name + " was killed as a result of " + name + "'s death");
            }
        }
    }

    public void SpawnDeathFX()
    {
        if (deathFX != null)
        {
            Instantiate(deathFX, transform.position, Quaternion.identity);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider != null && collision.gameObject.TryGetComponent(out Body other))
        {
            float damageAmount = collision.relativeVelocity.magnitude * rb.mass * collisionDamageMult;
            //if (Mathf.Round(damageAmount) > 0)
            //    Debug.Log(gameObject.name + ": Collided with " + collision.gameObject.name + "\nVelocity: " + collision.relativeVelocity.magnitude + ", Damage dealt: ~" + Mathf.Round(damageAmount));
            other.DealDamage(new Damage(damageAmount, Damage.Type.Kinetic, this));
        }
    }

    private void OnValidate()
    {
        if (!Application.isPlaying)
            hp = maxHP;
    }

    public void TemporarilyDisableCollider(float seconds)
    {
        StartCoroutine(DisableCollider(seconds));
    }

    private IEnumerator DisableCollider(float seconds)
    {
        col.enabled = false;
        yield return new WaitForSeconds(seconds);
        col.enabled = true;
    }
}
