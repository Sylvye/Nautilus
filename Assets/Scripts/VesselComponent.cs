using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public abstract class VesselComponent : Body
{
    public float respawnCooldown;
    public bool respawning = false;
    protected Rigidbody2D vesselRB;
    private SpriteRenderer sr;

    protected override void Awake()
    {
        base.Awake();
        vesselRB = GetComponentInParent<Rigidbody2D>();
        sr = GetComponentInParent<SpriteRenderer>();
    }

    public override bool OnDeath()
    {
        SpawnDeathFX();
        StartCoroutine(RespawnCoroutine());
        return true;
    }

    private IEnumerator RespawnCoroutine()
    {
        sr.enabled = false;
        col.enabled = false;
        respawning = true;
        yield return new WaitForSeconds(respawnCooldown);
        respawning = false;
        sr.enabled = true;
        col.enabled = true;
    }
}
