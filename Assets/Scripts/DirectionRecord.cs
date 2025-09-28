using Unity.VisualScripting;
using UnityEngine;

public class DirectionRecord : Record
{
    public float angle;
    public LineRenderer lr;
    private float lrWidth;

    protected override void Start()
    {
        base.Start();
        lrWidth = lr.startWidth;
    }

    public void SetAngle(float a)
    {
        angle = a;
        transform.rotation = Quaternion.Euler(Vector3.forward * a);
    }
    public override void SetColor(Color c)
    {
        base.SetColor(c);
        lr.startColor = c;
        lr.endColor = c;
    }

    protected override void Update()
    {
        base.Update();
        if (scaleWithCamera)
        {
            lr.startWidth = lrWidth * MapCameraController.main.zoomAmount;
            lr.endWidth = lrWidth * MapCameraController.main.zoomAmount;
        }
    }

    public override RecordType GetRecordType()
    {
        return RecordType.Direction;
    }
}
