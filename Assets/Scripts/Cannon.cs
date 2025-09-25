using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Cannon : VesselComponent
{
    public abstract void Activate(Vector2 dir);
    public abstract bool CanFire();
}
