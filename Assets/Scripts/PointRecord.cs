using Unity.VisualScripting;
using UnityEngine;

public class PointRecord : Record
{
    public override RecordType GetRecordType()
    {
        return RecordType.Point;
    }
}
