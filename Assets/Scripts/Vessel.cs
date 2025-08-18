using UnityEngine;
using System.Collections;

using System.Collections.Generic;
using UnityEditor.Rendering;

public class Vessel : Body
{
    public bool attacking;
    public List<Thruster> thrusters;
    public List<Cannon> cannons;
    //public AnimationCurve weightFalloffCurve;
    public float stabilizationRate;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (attacking)
        {
            foreach (Cannon c in cannons)
            {
                if (c.CanFire())
                    c.Activate();
            }
        }
    }

    public void Move(Vector2 input)
    {
        Move(input, 1);
    }

    public void Move(Vector2 input, float mult)
    {
        float desiredAngle = AngleHelper.VectorToDegrees(input);
        float rotationOffset = transform.eulerAngles.z;
        //Debug.DrawLine(transform.position, transform.position + (Vector3)AngleHelper.DegreesToVector(desiredAngle + rotationOffset) * 2, Color.blue);
        foreach (Thruster t in thrusters)
        {
            float thrusterAngle = AngleHelper.VectorToDegrees(-t.transform.localPosition);

            float difference = Mathf.Abs(Mathf.DeltaAngle(desiredAngle, thrusterAngle));
            if (difference < 90)
            {
                float power = Mathf.Cos(difference * Mathf.Deg2Rad) * mult;
                t.Activate(power);
            }
        }
    }

    public void Stabilize()
    {
        float angVel = Mathf.Abs(rb.angularVelocity);
        if (angVel > 5)
        {
            Move(rb.angularVelocity > 0 ? Vector2.right : Vector2.left, Mathf.Clamp(angVel / stabilizationRate, 0, 2));
        }
        else
        {
            rb.angularVelocity = 0;
        }
    }
}
