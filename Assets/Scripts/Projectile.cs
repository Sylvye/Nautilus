using UnityEngine;

public interface Projectile
{
    public void SetSource(GameObject source);

    public GameObject GetSource();

    public void SetAimPos(Vector2 aimPos);

    public Vector2 GetAimPos();
}
