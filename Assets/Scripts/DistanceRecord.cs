using UnityEngine;

public class DistanceRecord : MonoBehaviour, Record
{
    public Vector2 position;
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
}
