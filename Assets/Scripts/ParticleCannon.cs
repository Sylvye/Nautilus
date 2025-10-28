using UnityEngine;

public class ParticleCannon : VesselComponent, Cannon
{
    public ParticleSystem ps;
    public int rate;
    public float kickbackMult;

    public void Activate(Vector2 aimPos, GameObject source)
    {
        Vector2 dir = aimPos - (Vector2)transform.position;
        dir.Normalize();

        transform.rotation = Quaternion.Euler(0, 0, AngleHelper.VectorToDegrees(dir));
        ps.Emit(rate);
        if (kickbackMult != 0)
        {
            float flatMult = 10;
            Vector2 forceDir = -dir.normalized;
            Vector2 force = flatMult * kickbackMult * 5 * forceDir;
            Vector2 relative = (Vector2)transform.position - vesselRB.worldCenterOfMass;
            float torque = relative.x * force.y - relative.y * force.x;
            vesselRB.AddForce(force, ForceMode2D.Force);
            vesselRB.AddTorque(torque, ForceMode2D.Force);
            Debug.DrawLine(transform.position, transform.position - (Vector3)force, Color.purple);
        }
    }

    public bool CanFire()
    {
        return true;
    }
}
