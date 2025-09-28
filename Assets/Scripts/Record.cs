using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public abstract class Record : MonoBehaviour
{
    public Material m;
    public float zOffset;
    public Vector2 position;
    public string label;
    public Color color;
    public bool cameraScale;
    private Vector3 defaultScale;

    private void Awake()
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

    public void SetPosition(Vector3 pos)
    {
        position = pos;
        transform.position = pos + Vector3.forward * zOffset;
    }

    public void SetLabel(string l)
    {
        label = l;
    }

    public void SetColor(Color c)
    {
        color = c;
        m.SetColor("_Color", c);
    }

    public virtual void Update()
    {
        if (cameraScale)
        {
            transform.localScale = defaultScale * MapCameraController.main.zoomAmount;
        }
    }

    public virtual RecordType GetRecordType()
    {
        return RecordType.None;
    }
}