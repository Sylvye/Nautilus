using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CameraController : MonoBehaviour
{
    public CameraController main;
    public float lerpSpeed;
    private Transform playerTransform;

    void Awake()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        float y = playerTransform.position.y;
        if (y <= 0)
            y = 0;

        transform.position = Vector3.Lerp(transform.position, new Vector3(0, y, -10), lerpSpeed * Time.deltaTime);
    }
}
