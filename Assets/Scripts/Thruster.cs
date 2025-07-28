using Unity.VisualScripting;
using UnityEngine;

public class Thruster : MonoBehaviour
{
    public float power;
    private Rigidbody2D parentRB;
    private Gradient g;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        parentRB = transform.parent.GetComponent<Rigidbody2D>();
        g = new Gradient();
        g.colorKeys = new GradientColorKey[] { new GradientColorKey(Color.yellowGreen, 0), new GradientColorKey(Color.orange, 0.5f), new GradientColorKey(Color.red, 1) };
    }

    public void Fire(float ratio)
    {
        Vector2 forceVector = AngleHelper.DegreesToVector(transform.eulerAngles.z + 90).normalized;
        Vector2 force = forceVector * power * ratio;
        Vector2 relative = (Vector2)transform.position - parentRB.worldCenterOfMass;
        float torque = relative.x * force.y - relative.y * force.x;
        parentRB.AddForce(force);
        parentRB.AddTorque(torque);
        Debug.DrawLine(transform.position, transform.position - (Vector3)force, g.Evaluate(Mathf.Clamp(ratio, 0, 1)));
    }
}
