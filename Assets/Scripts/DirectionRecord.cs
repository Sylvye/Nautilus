using Unity.VisualScripting;
using UnityEngine;

public class DirectionRecord : Record
{
    public float angle;
    public LineRenderer lr;

    public void SetAngle(float a)
    {
        angle = a;
        transform.rotation = Quaternion.Euler(Vector3.forward * a);
    }

    public void OnValidate()
    {
        SetAngle(angle);
    }

    public override RecordType GetRecordType()
    {
        return RecordType.Direction;
    }
}
