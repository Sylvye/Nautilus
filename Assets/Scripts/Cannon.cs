using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Cannon : VesselComponent
{
    public bool mouseAim;
    public abstract void Activate();
    public abstract bool CanFire();

    private void Update()
    {
        if (mouseAim)
        {
            transform.eulerAngles = new Vector3(0, 0, AngleHelper.VectorToDegrees(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - transform.position));
        }
    }
}
