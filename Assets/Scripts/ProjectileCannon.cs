using UnityEngine;
using UnityEngine.InputSystem;

public class ProjectileCannon : Cannon
{
    public Projectile proj;
    public float attackSpeed;
    public float power;
    private float nextAttack;

    public override void Activate()
    {
        if (!respawning)
        {
            Vector2 dir = AngleHelper.DegreesToVector(transform.eulerAngles.z + 90);

            Launch(proj.gameObject, dir, power, 0.1f);
            nextAttack = Time.time + 1 / attackSpeed;
        }
    }

    public override bool CanFire()
    {
        return Time.time >= nextAttack;
    }

    public void Launch(GameObject obj, Vector2 dir, float power, float spawnDist)
    {
        dir.Normalize();

        GameObject instance = Instantiate(obj.gameObject, (Vector2)transform.position + dir * spawnDist, Quaternion.identity);
        Rigidbody2D rb = instance.GetComponent<Rigidbody2D>();
        rb.linearVelocity = dir * power + vesselRB.linearVelocity;
    }
}
