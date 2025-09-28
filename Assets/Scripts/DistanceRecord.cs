using Unity.VisualScripting;
using UnityEngine;

public class DistanceRecord : RadiusRecord
{
    public override RecordType GetRecordType()
    {
        return RecordType.Distance;
    }
}
