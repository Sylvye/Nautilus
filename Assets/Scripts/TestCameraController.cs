using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(Camera))]
public class TestCameraController : MonoBehaviour
{
    [Tooltip("Objects to focus on")]
    public List<GameObject> targets = new List<GameObject>();

    [Tooltip("Padding around the objects")]
    public float padding = 2f;

    [Tooltip("Minimum orthographic size for the camera")]
    public float minSize = 5f;

    private Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        if (targets == null || targets.Count == 0 || cam == null)
            return;

        Bounds bounds = CalculateBounds();
        CenterCamera(bounds.center);

        if (cam.orthographic)
        {
            AdjustOrthographicSize(bounds);
        }
        else
        {
            Debug.LogWarning("This script currently supports orthographic cameras only.");
        }
    }

    private Bounds CalculateBounds()
    {
        Vector3 average = Vector3.zero;
        int validCount = 0;

        foreach (var obj in targets)
        {
            if (obj == null) continue;
            average += obj.transform.position;
            validCount++;
        }

        if (validCount == 0)
            return new Bounds(Vector3.zero, Vector3.one);

        average /= validCount;

        Bounds bounds = new Bounds(average, Vector3.zero);

        foreach (var obj in targets)
        {
            if (obj == null) continue;
            bounds.Encapsulate(obj.transform.position);
        }

        return bounds;
    }

    private void CenterCamera(Vector3 center)
    {
        if (cam.orthographic)
        {
            transform.position = new Vector3(center.x, center.y, transform.position.z);
        }
        else
        {
            // For future 3D perspective logic
        }
    }

    private void AdjustOrthographicSize(Bounds bounds)
    {
        float screenRatio = (float)Screen.width / Screen.height;
        float targetRatio = bounds.size.x / bounds.size.y;

        float size = bounds.extents.y;
        if (targetRatio > screenRatio)
        {
            size = bounds.extents.x / screenRatio;
        }

        cam.orthographicSize = Mathf.Max(size + padding, minSize);
    }
}
