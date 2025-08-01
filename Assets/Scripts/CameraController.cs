using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CameraController : MonoBehaviour
{
    public static CameraController main;
    public float followSpeed;
    public float scaleSpeed;
    public float ssFalloff;
    private float ssIntensity;
    private Vector2 lerpPos;
    private Vector2 refRes;
    private float targetSize;
    private float currentSize;
    private Transform playerTransform;
    private PixelPerfectCamera ppc;

    void Awake()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
        main = this;
        ppc = GetComponent<PixelPerfectCamera>();
        refRes = new Vector2(ppc.refResolutionX, ppc.refResolutionY);
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
        ppc.refResolutionX = (int)(refRes.x * size);
        ppc.refResolutionY = (int)(refRes.y * size);
    }
}
