using Unity.VisualScripting;
using UnityEngine;

public class Thruster : VesselComponent
{
    public float power;
    public float forceMult;
    public ParticleSystem fireFX;
    private float startSpeedMin;
    private float startSpeedMax;
    private float startSizeMin;
    private float startSizeMax;
    private float startAngle;
    private Vector3 lastFireFXPosition;
    private Vector3 currentFireFXPosition;
    private Vector3 localFireFXHomePosition;
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
        lastFireFXPosition = fireFX.transform.position;
        currentFireFXPosition = fireFX.transform.position;
        localFireFXHomePosition = fireFX.transform.localPosition;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        lastFireFXPosition = currentFireFXPosition;
        currentFireFXPosition = fireFX.transform.position;
    }

    public void Activate(float ratio)
    {
        if (respawning)
            return;

        ratio = Mathf.Clamp(ratio, 0, 2);
        float mult = power * ratio;
        float finalMult = Mathf.Clamp(Mathf.Sqrt(mult), 0.5f, 10);

        var fireFXmain = fireFX.main;
        var fireFXShape = fireFX.shape;
        fireFXmain.startSpeed = new ParticleSystem.MinMaxCurve(startSpeedMin * finalMult, startSpeedMax * finalMult);
        fireFXmain.startSize = new ParticleSystem.MinMaxCurve(startSizeMin * finalMult, startSizeMax * finalMult);
        fireFXShape.angle = startAngle * ratio;

        EmitAlongPath(mult * 5);

        float flatMult = 10;
        Vector2 forceDir = AngleHelper.DegreesToVector(transform.eulerAngles.z + 90).normalized;
        Vector2 force = mult * flatMult * forceMult * forceDir;
        Vector2 relative = (Vector2)transform.position - vesselRB.worldCenterOfMass;
        float torque = relative.x * force.y - relative.y * force.x;
        vesselRB.AddForce(force, ForceMode2D.Force);
        vesselRB.AddTorque(torque, ForceMode2D.Force);

        // Show force vectors
        //Debug.DrawLine(transform.position, transform.position - (Vector3)force / flatMult, g.Evaluate(Mathf.Clamp(ratio, 0, 1)));
    }

    private void EmitAlongPath(float particleCount)
    {
        Vector3 direction = currentFireFXPosition - lastFireFXPosition;
        float distance = direction.magnitude;

        if (distance <= 0f)
        {
            fireFX.Emit((int)particleCount);
            return;
        }

        int steps = Mathf.CeilToInt(distance*50);
        steps = Mathf.Max(steps, 1);

        Vector3 stepVector = direction / steps;

        for (int i = 0; i < steps; i++)
        {
            fireFX.transform.position = lastFireFXPosition + stepVector * i;
            fireFX.Emit(Mathf.CeilToInt(particleCount / steps));
        }

        fireFX.transform.localPosition = localFireFXHomePosition;
    }
}
