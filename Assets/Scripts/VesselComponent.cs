using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class VesselComponent : Body
{
    public float respawnCooldown;
    public bool componentCollisions;
    public bool parentCollisions;
    [NonSerialized] public bool respawning = false;
    [NonSerialized] public Vessel parentVessel;
    protected Rigidbody2D vesselRB;
    private SpriteRenderer sr;

    protected override void Awake()
    {
        base.Awake();
        vesselRB = GetComponentInParent<Rigidbody2D>();
        sr = GetComponentInParent<SpriteRenderer>();
        parentVessel = GetComponentInParent<Vessel>();
    }

    protected virtual void Start()
    {
        if (!componentCollisions)
        {
            for (int i = 0; i < transform.parent.childCount; i++)
            {
                Transform t = transform.parent.GetChild(i);
                if (t.TryGetComponent(out Collider2D cc))
                {
                    Physics2D.IgnoreCollision(col, cc);
                }
            }
        }

        if (!parentCollisions && TryGetComponent(out Collider2D c))
        {
            Physics2D.IgnoreCollision(col, c);
        }

        List<Resistance> resistanceProducts = new();
        foreach (Resistance r in resistances)
        {
            Resistance newR = new Resistance(r.type, r.mult);

            Predicate<Resistance> predR = x => x.type == r.type;
            if (resistances.Exists(predR))
            {
                Resistance r2 = resistances.Find(predR);
                if (r2.inherit)
                    newR.mult *= r2.mult;
            }

            resistanceProducts.Add(newR);
        }
    }

    public override bool OnDeath()
    {
        SpawnDeathFX();
        KillChildren();
        if (respawnCooldown > 0)
            StartCoroutine(RespawnCoroutine());
        else
            Destroy(gameObject);
        return true;
    }

    private IEnumerator RespawnCoroutine()
    {
        sr.enabled = false;
        if (col != null)
            col.enabled = false;
        respawning = true;
        yield return new WaitForSeconds(respawnCooldown);
        respawning = false;
        sr.enabled = true;
        if (col != null)
            col.enabled = true;
        hp = maxHP;
    }
}
