using UnityEngine;

public class Tentacle : VesselComponent
{
    public float wiggleFreq;
    public float wiggleAmp;

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        rb.angularVelocity += Mathf.Sin(Time.time * wiggleFreq) * wiggleAmp;
    }
}
