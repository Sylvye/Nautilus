using UnityEngine;
using UnityEngine.InputSystem;

public class Launcher : MonoBehaviour
{
    public void Launch(GameObject obj, Vector2 dir, float power, float spawnDist, Transform parent)
    {
        dir.Normalize();

        GameObject instance = Instantiate(obj.gameObject, (Vector2)transform.position + dir, Quaternion.identity);
        instance.transform.parent = parent;
        Rigidbody2D rb = instance.GetComponent<Rigidbody2D>();
        rb.linearVelocity = dir * power;
    }
}
