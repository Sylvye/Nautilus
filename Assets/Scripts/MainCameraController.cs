using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class MainCameraController : MonoBehaviour
{
    public static MainCameraController main;
    public float followSpeed;
    public float scaleSpeed;
    public float ssFalloff;
    private float ssIntensity;
    private Vector2 lerpPos;
    private float startSize;
    private float targetSize;
    private float currentSize;
    private Transform playerTransform;
    private Camera c;
    private Material backgroundMaterial;

    void Awake()
    {
        main = this;
        playerTransform = GameObject.FindWithTag("Player").transform;
        c = GetComponent<Camera>();
        startSize = c.orthographicSize;
        currentSize = 1;
        targetSize = 1;
        backgroundMaterial = GameObject.Find("Background").GetComponent<SpriteRenderer>().material;
    }

    private void Update()
    {
        if (playerTransform != null)
            lerpPos = Vector2.Lerp(lerpPos, playerTransform.position, followSpeed * Time.deltaTime);
        Vector2 ssOffset = AngleHelper.DegreesToVector(Random.Range(0, 360f)) * ssIntensity;
        ssIntensity = Mathf.Lerp(ssIntensity, 0, ssFalloff * Time.deltaTime);
        transform.position = (Vector3)lerpPos + (Vector3)ssOffset + Vector3.back * 10;
        currentSize = Mathf.Lerp(currentSize, targetSize, scaleSpeed * Time.deltaTime);
        SetRefResolutionSize(currentSize); // TEST THIS!!!!
        backgroundMaterial.SetVector("_Offset", (Vector2)transform.position);
    }

    public void ScreenShake(float intensity)
    {
        ssIntensity += intensity;
    }

    public void SetSize(float size)
    {
        targetSize = size;
    }

    private void SetRefResolutionSize(float size)
    {
        c.orthographicSize = startSize*size;
    }
}
