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

    private void Update()
    {
        Debug.Log(rb.angularVelocity);
    }

    public void Move(Vector2 input)
    {
        float desiredAngle = AngleHelper.VectorToRadians(input);
        foreach (Thruster t in thrusters)
        {
            float thrusterAngle = AngleHelper.VectorToRadians(-t.transform.localPosition);

            float difference = Mathf.Abs(desiredAngle - thrusterAngle);
            if (difference > Mathf.PI / 2)
            {
                continue;
            }
            float power = Mathf.Cos(difference);
            t.Fire(power);
        }
        float rotationOffset = transform.eulerAngles.z * Mathf.Deg2Rad;
        Debug.DrawLine(transform.position, transform.position + (Vector3)AngleHelper.RadiansToVector(desiredAngle + rotationOffset) * 2, Color.yellow);
    }
}
