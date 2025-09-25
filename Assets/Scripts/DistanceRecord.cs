using Unity.VisualScripting;
using UnityEngine;

public class DistanceRecord : Record
{
    public float radius;

    public void SetRadius(float r)
    {
        float d = r * 2;
        radius = r;
        transform.localScale = new Vector3(d,d,d);
    }

    public void OnValidate()
    {
        SetRadius(radius);
    }

    public override RecordType GetRecordType()
    {
        return RecordType.Distance;
    }
}
