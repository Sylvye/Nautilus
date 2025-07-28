using Unity.VisualScripting;
using UnityEngine;

public class Thruster : MonoBehaviour
{
    public float power;
    private Joint2D j;
    private Rigidbody2D parentRB;
    private Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        parentRB = transform.parent.GetComponent<Rigidbody2D>();
        rb = GetComponent<Rigidbody2D>();
        j = GetComponent<Joint2D>();
    }

    private void Start()
    {
        j.connectedBody = parentRB;
    }

    public void Fire(float ratio)
    {
        Vector2 forceVector = AngleHelper.DegreesToVector(transform.eulerAngles.z + 90).normalized;
        Vector2 force = forceVector * power * ratio;
        rb.AddForce(force);
        Debug.DrawLine(transform.position, transform.position - (Vector3)force, Color.red);
    }
}
