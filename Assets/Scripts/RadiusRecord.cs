using UnityEngine;

public abstract class RadiusRecord : Record
{
    public float radius;
    public bool scaleThicknessWithCamera;
    private float startThickness;

    protected override void Awake()
    {
        base.Awake();
        startThickness = m.GetFloat("_Thickness");
    }

    protected override void Update()
    {
        base.Update();
        m.SetFloat("_Thickness", 100 / radius * startThickness * MapCameraController.main.zoomAmount); // scaling coefficient of 100
    }

    public void SetRadius(float r)
    {
        radius = r;
        float d = r * 2;
        transform.localScale = new Vector3(d, d, d);
    }

    public void OnValidate()
    {
        SetRadius(radius);
    }
}
