using UnityEngine;
using UnityEngine.InputSystem;

public interface Cannon
{
    public void Activate(Vector2 aimPos, GameObject source);

    public bool CanFire();
}
