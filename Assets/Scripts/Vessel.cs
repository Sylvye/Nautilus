using UnityEngine;
using System.Collections;

using System.Collections.Generic;
using UnityEditor.Rendering;

public class Vessel : MonoBehaviour
{
    public List<Thruster> thrusters;
    public List<Cannon> cannons;
    public AnimationCurve weightFalloffCurve;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Move(Vector2 input)
    {
        Move(input, 1);
    }

    public void Move(Vector2 input, float mult)
    {
        float desiredAngle = AngleHelper.VectorToDegrees(input);
        float rotationOffset = transform.eulerAngles.z;
        Debug.DrawLine(transform.position, transform.position + (Vector3)AngleHelper.DegreesToVector(desiredAngle + rotationOffset) * 2, Color.blue);
        foreach (Thruster t in thrusters)
        {
            float thrusterAngle = AngleHelper.VectorToDegrees(-t.transform.localPosition);

            float difference = Mathf.Abs(Mathf.DeltaAngle(desiredAngle, thrusterAngle));
            if (difference > 90)
            {
                continue;
            }
            float power = Mathf.Cos(difference * Mathf.Deg2Rad) * mult;
            t.Fire(power);
        }
    }

    public void Stabilize(float power)
    {
        if (rb.angularVelocity > 0)
        {

        }
        else if (rb.angularVelocity < 0)
        {

        }
    }
}
