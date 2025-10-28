using System.Collections;
using System.Runtime.InteropServices;
using UnityEditor.Rendering;
using UnityEngine;

public class TorpedoController : MonoBehaviour, Targetable, Projectile
{
    public LayerMask targettingMask;
    public GameObject target;
    public GameObject source;
    public Vector2 aimPos;
    public float rotateSpeed;
    public float immunityTime;
    public float lifetime;
    private Vessel v;
    private Rigidbody2D rb;
    private Collider2D col;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        v = GetComponent<Vessel>();
        col = GetComponent<Collider2D>();
    }

    private void Start()
    {
        foreach (Transform comp in transform) // temporarily disable collisions for all components
        {
            if (comp.TryGetComponent(out VesselComponent vc))
            {
                vc.TemporarilyDisableCollider(immunityTime);
            }
        }

        v.TemporarilyDisableCollider(immunityTime);

        Collider2D hit = Physics2D.OverlapPoint(aimPos, targettingMask);
        if (hit != null)
        {
            target = hit.gameObject;
        }
        StartCoroutine(DestroyAfter(lifetime));
    }

    private void FixedUpdate()
    {
        if (target != null)
        {
            Vector2 dir = target.transform.position - transform.position;
            float targetAngle = AngleHelper.VectorToDegrees(dir);
            float facingAngle = transform.eulerAngles.z + 90;
            float angleDiff = Mathf.DeltaAngle(facingAngle, targetAngle);
            rb.AddTorque(Mathf.Sign(angleDiff) * rotateSpeed, ForceMode2D.Force);
        }
        v.Move(Vector2.up);
    }

    public void SetTarget(GameObject obj)
    {
        target = obj;
    }

    public GameObject GetTarget()
    {
        return target;
    }

    public void SetSource(GameObject obj)
    {
        source = obj;
    }

    public GameObject GetSource()
    {
        return source;
    }

    public void SetAimPos(Vector2 aim)
    {
        aimPos = aim;
    }

    public Vector2 GetAimPos()
    {
        return aimPos;
    }

    private IEnumerator DestroyAfter(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        v.OnDeath();
    }
}
