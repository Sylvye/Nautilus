using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float acceleration;
    public float baseDamage;
    public float lifetime;
    private Rigidbody2D rb;
    private Collider2D col;
    private float despawnTime;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        despawnTime = Time.time + lifetime;
    }

    private void FixedUpdate()
    {
        rb.AddForce(AngleHelper.DegreesToVector(transform.eulerAngles.z) * acceleration);
        
        if (Time.time >= despawnTime)
        {
            Destroy(gameObject);
        }
    }

    public void OnCollide(Vessel hit)
    {
        Destroy(gameObject);
        hit.Damage(baseDamage * rb.linearVelocity.magnitude);
    }
}
