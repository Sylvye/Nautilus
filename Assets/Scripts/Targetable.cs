using UnityEngine;

public interface Targetable
{
    public void SetTarget(GameObject target);

    public GameObject GetTarget();
}
