using UnityEngine;

public abstract class Cannon : VesselComponent
{
    public abstract void Activate();
    public abstract bool CanFire();
}
