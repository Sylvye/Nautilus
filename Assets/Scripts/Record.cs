using Unity.VisualScripting;
using UnityEngine;

public abstract class Record : MonoBehaviour
{
    public Material m;
    public float zOffset;

    private void Awake()
    {
        m = GetComponent<Renderer>().material;
    }

    public enum RecordType
    {
        None,
        Point,
        Direction,
        Area,
        Distance
    }

    public Vector2 Position
    {
        get => Position;
        set => SetPosition(value);
    }
    public string Label
    {
        get => Label;
        set => SetLabel(value);
    }
    public Color Color
    {
        get => Color;
        set => SetColor(value);
    }

    public void SetPosition(Vector3 pos)
    {
        Position = pos;
        transform.position = pos;
    }

    public void SetLabel(string l)
    {
        Label = l;
    }

    public void SetColor(Color c)
    {
        Color = c;
        m.SetColor("Color", c);
    }

    public virtual RecordType GetRecordType()
    {
        return RecordType.None;
    }
}