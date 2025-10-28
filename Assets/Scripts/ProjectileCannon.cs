using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;

public class ProjectileCannon : VesselComponent, Cannon
{
    public GameObject proj;
    public float attackSpeed;
    public float power;
    public float spawnDist;
    public bool inheritVelocity;
    private float nextAttack;

    public void Activate(Vector2 aimPos, GameObject source)
    {
        if (!respawning)
        {
            Launch(proj, source, aimPos, power, spawnDist);
            nextAttack = Time.time + 1 / attackSpeed;
        }
    }

    public bool CanFire()
    {
        return Time.time >= nextAttack;
    }

    public void Launch(GameObject obj, GameObject source, Vector2 aimPos, float power, float spawnDist)
    {
        Debug.DrawLine(transform.position, aimPos, Color.red, 1f);
        Vector2 dir = aimPos - (Vector2)transform.position;
        dir.Normalize();

        GameObject instance = Instantiate(obj, (Vector2)transform.position + dir * spawnDist, Quaternion.identity);
        instance.transform.eulerAngles = Vector3.forward * (AngleHelper.VectorToDegrees(dir)-90);
        Rigidbody2D rb = instance.GetComponent<Rigidbody2D>();
        rb.linearVelocity = dir * power;
        if (inheritVelocity)
        {
            rb.linearVelocity += vesselRB.linearVelocity;
        }
        if (instance.TryGetComponent(out Projectile p))
        {
            p.SetSource(source);
            p.SetAimPos(aimPos);
        }
    }
}
