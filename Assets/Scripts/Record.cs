using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public abstract class Record : MonoBehaviour
{
    public float zOffset;
    public Vector2 position;
    public string label;
    public Color color;
    public bool scaleWithCamera;
    protected Material m;
    private Vector3 defaultScale;

    protected virtual void Awake()
    {
        m = GetComponent<Renderer>().material;
    }

    protected virtual void Start()
    {
        defaultScale = transform.localScale;
    }

    public enum RecordType
    {
        None,
        Point,
        Direction,
        Distance,
        Area
    }

    public virtual void SetPosition(Vector3 pos)
    {
        position = pos;
        transform.position = pos + Vector3.forward * zOffset;
    }

    public virtual void SetLabel(string l)
    {
        label = l;
    }

    public virtual void SetColor(Color c)
    {
        color = c;
        m.SetColor("_Color", c);
    }

    protected virtual void Update()
    {
        if (scaleWithCamera)
        {
            transform.localScale = defaultScale * MapCameraController.main.zoomAmount;
        }
    }

    public virtual RecordType GetRecordType()
    {
        return RecordType.None;
    }
}