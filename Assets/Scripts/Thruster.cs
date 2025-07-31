using Unity.VisualScripting;
using UnityEngine;

public class Thruster : VesselComponent
{
    public float power;
    public ParticleSystem fireFX;
    private float startSpeedMin;
    private float startSpeedMax;
    private float startSizeMin;
    private float startSizeMax;
    private float startAngle;
    private Gradient g;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Awake()
    {
        base.Awake();
        vesselRB = transform.parent.GetComponent<Rigidbody2D>();
        g = new Gradient();
        g.colorKeys = new GradientColorKey[] { new GradientColorKey(Color.yellowGreen, 0), new GradientColorKey(Color.orange, 0.5f), new GradientColorKey(Color.red, 1) };
        startSpeedMin = fireFX.main.startSpeed.constantMin;
        startSpeedMax = fireFX.main.startSpeed.constantMax;
        startSizeMin = fireFX.main.startSize.constantMin;
        startSizeMax = fireFX.main.startSize.constantMax;
        startAngle = fireFX.shape.angle;
    }

    public void Activate(float ratio)
    {
        if (!respawning)
        {
            float mult = power * ratio;
            float finalMult = Mathf.Clamp(Mathf.Sqrt(mult), 0.5f, 10);
            var fireFXmain = fireFX.main;
            var fireFXShape = fireFX.shape;
            fireFXmain.startSpeed = new ParticleSystem.MinMaxCurve(startSpeedMin * finalMult, startSpeedMax * finalMult);
            fireFXmain.startSize = new ParticleSystem.MinMaxCurve(startSizeMin * finalMult, startSizeMax * finalMult);
            fireFXShape.angle = startAngle * (1 + Mathf.Clamp(ratio - 1, 0, 2));

            fireFX.Emit((int)(mult * 5));

            float flatMult = 10;
            Vector2 forceDir = AngleHelper.DegreesToVector(transform.eulerAngles.z + 90).normalized;
            Vector2 force = mult * flatMult * forceDir;
            Vector2 relative = (Vector2)transform.position - vesselRB.worldCenterOfMass;
            float torque = relative.x * force.y - relative.y * force.x;
            vesselRB.AddForce(force, ForceMode2D.Force);
            vesselRB.AddTorque(torque, ForceMode2D.Force);
            Debug.DrawLine(transform.position, transform.position - (Vector3)force, g.Evaluate(Mathf.Clamp(ratio, 0, 1)));
        }
    }
}
