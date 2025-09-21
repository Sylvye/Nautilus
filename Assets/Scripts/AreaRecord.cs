using UnityEngine;

public class AreaRecord : MonoBehaviour, Record
{
    public Vector2 position;
    public float radius;

    public void SetRadius(float r)
    {
        float d = r * 2;
        transform.localScale = new Vector3(d, d, d);
    }

    public void OnValidate()
    {
        SetRadius(radius);
    }
}
