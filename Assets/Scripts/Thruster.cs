using UnityEngine;

public class Thruster : MonoBehaviour
{
    public float power;
    private Rigidbody2D parentRB;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        parentRB = transform.parent.GetComponent<Rigidbody2D>();
    }

    public void Fire(float ratio)
    {
        Vector2 force = (transform.parent.position - transform.position).normalized * power * ratio;
        parentRB.AddForce(force);
    }
}
