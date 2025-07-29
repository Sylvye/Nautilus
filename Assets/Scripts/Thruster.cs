using Unity.VisualScripting;
using UnityEngine;

public class Thruster : MonoBehaviour
{
    public float power;
    public ParticleSystem fireFX;
    private float startSpeedMin;
    private float startSpeedMax;
    private float startSizeMin;
    private float startSizeMax;
    private Rigidbody2D parentRB;
    private Gradient g;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        parentRB = transform.parent.GetComponent<Rigidbody2D>();
        g = new Gradient();
        g.colorKeys = new GradientColorKey[] { new GradientColorKey(Color.yellowGreen, 0), new GradientColorKey(Color.orange, 0.5f), new GradientColorKey(Color.red, 1) };
        startSpeedMin = fireFX.main.startSpeed.constantMin;
        startSpeedMax = fireFX.main.startSpeed.constantMax;
        startSizeMin = fireFX.main.startSize.constantMin;
        startSizeMax = fireFX.main.startSize.constantMax;
    }

    public void Fire(float ratio)
    {
        float mult = power * ratio;
        float finalMult = Mathf.Clamp(Mathf.Sqrt(mult), 0.5f, 10);
        var fireFXmain = fireFX.main;
        fireFXmain.startSpeed = new ParticleSystem.MinMaxCurve(startSpeedMin * finalMult, startSpeedMax * finalMult);
        fireFXmain.startSize = new ParticleSystem.MinMaxCurve(startSizeMin * finalMult, startSizeMax * finalMult);
        fireFX.Emit((int)(mult * 5));

        Vector2 forceVector = AngleHelper.DegreesToVector(transform.eulerAngles.z + 90).normalized;
        Vector2 force = forceVector * mult;
        Vector2 relative = (Vector2)transform.position - parentRB.worldCenterOfMass;
        float torque = relative.x * force.y - relative.y * force.x;
        parentRB.AddForce(force);
        parentRB.AddTorque(torque);
        Debug.DrawLine(transform.position, transform.position - (Vector3)force, g.Evaluate(Mathf.Clamp(ratio, 0, 1)));
    }
}
