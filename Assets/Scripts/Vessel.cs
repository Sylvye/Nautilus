using UnityEngine;
using System.Collections;

using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;

public class Vessel : Body
{
    public float stabilizationRate;
    [DoNotSerialize]public List<Thruster> thrusters;
    [DoNotSerialize]public List<Cannon> cannons;

    protected override void Awake()
    {
        base.Awake();
    }

    protected virtual void Start()
    {
        thrusters = GetComponentsInChildren<Thruster>().ToList();
        cannons = GetComponentsInChildren<Cannon>().ToList();
    }

    public void Move(Vector2 input)
    {
        Move(input, 1);
    }

    public void Move(Vector2 input, float mult)
    {
        float desiredAngle = AngleHelper.VectorToDegrees(input);
        foreach (Thruster t in thrusters)
        {
            if (t != null)
            {
                if (input != Vector2.zero)
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
