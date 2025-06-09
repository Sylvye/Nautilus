using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public float jump;
    private Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");

        rb.AddForce(Vector2.right * speed, ForceMode2D.Force);

        if (Input.GetButtonDown("Jump"))
        {
            rb.AddForce(Vector2.up * jump, ForceMode2D.Force);
        }
    }

    
}
