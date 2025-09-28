using Unity.VisualScripting;
using UnityEngine;

public class AreaRecord : RadiusRecord
{
    public override RecordType GetRecordType()
    {
        return RecordType.Area;
    }
}
