using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public abstract class Cell : MonoBehaviour
{
    public bool locked;
    public int connections = 0;
    public int maxConnections = 3;

    public float snapRadius;
    public float lockedRadius;

    public GameObject connector;
    private Transform connectorParent;

    private HashSet<GameObject> skipObjs;
    [SerializeField]
    private List<Cell> connected;

    private Rigidbody2D rb;
    private Gimbal gimbal;
    private LayerMask cellLayerMask;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        cellLayerMask = LayerMask.GetMask("Cell");
        skipObjs = new HashSet<GameObject>();
        connected = new List<Cell>();
        connectorParent = GameObject.Find("Connectors").transform;
        gimbal = GetComponentInChildren<Gimbal>();
    }

    private void Start()
    {
        skipObjs.Add(gameObject);
    }

    void FixedUpdate()
    {
        if (connections < maxConnections)
        {
            RaycastHit2D[] hit = Physics2D.CircleCastAll(transform.position, snapRadius, Vector2.zero, 0, cellLayerMask);
            float snapWindow = 0.1f;

            if (locked)
            {
                foreach (RaycastHit2D h in hit)
                {
                    if (h.collider != null && !skipObjs.Contains(h.transform.gameObject) && Mathf.Abs(Vector2.Distance(transform.position, h.transform.position) - snapRadius) < snapWindow) // ensures it's the right distance away from the other cell
                    {
                        Cell other = h.transform.gameObject.GetComponent<Cell>();

                        if (!other.locked)
                            continue; // dont connect to unlocked nodes

                        skipObjs.Add(other.gameObject);
                        other.skipObjs.Add(gameObject);

                        if (other.connections >= other.maxConnections)
                            continue;

                        connections++;
                        connected.Add(other);
                        skipObjs.Add(other.gameObject);
                        other.connections++;
                        other.connected.Add(this);
                        other.skipObjs.Add(gameObject);

                        OnSnap(other);

                        // spawn connector
                        GameObject connObj = Instantiate(connector, connectorParent);
                        CellConnector cConn = connObj.GetComponent<CellConnector>();
                        cConn.c1 = gameObject;
                        cConn.c2 = other.gameObject;
                    }
                }
            }
            else
            {
                foreach (RaycastHit2D h in hit)
                {
                    if (h.collider != null && !skipObjs.Contains(h.transform.gameObject) && Mathf.Abs(Vector2.Distance(transform.position, h.transform.position) - snapRadius) < snapWindow) // ensures it's the right distance away from the other cell
                    {
                        Cell other = h.transform.gameObject.GetComponent<Cell>();

                        if (other.connections >= other.maxConnections)
                        {
                            skipObjs.Add(other.gameObject);
                            other.skipObjs.Add(gameObject);
                            continue;
                        }

                        connections++;
                        connected.Add(other);
                        skipObjs.Add(other.gameObject);
                        other.connections++;
                        other.connected.Add(this);
                        other.skipObjs.Add(gameObject);
                        if (!other.locked)
                            other.LockTo(transform);

                        LockTo(other.transform);

                        // positions the node to the right distance
                        Vector2 dir = (other.transform.position - transform.position).normalized;
                        Vector2 snapPosition = (Vector2)other.transform.position - dir * snapRadius;
                        transform.position = snapPosition;

                        OnSnap(other);

                        // spawn connector
                        GameObject connObj = Instantiate(connector, connectorParent);
                        CellConnector cConn = connObj.GetComponent<CellConnector>();
                        cConn.c1 = gameObject;
                        cConn.c2 = other.gameObject;
                    }
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!locked && collision.gameObject.layer == LayerMask.NameToLayer("Terrain"))
        {
            LockTo(collision.transform);
            FixedJoint2D joint = gameObject.AddComponent<FixedJoint2D>();
            joint.connectedBody = collision.transform.GetComponent<Rigidbody2D>();
            joint.frequency = 100;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;

            //rb.bodyType = RigidbodyType2D.Static;
        }
    }

    private void LockTo(Transform other)
    {
        locked = true;
        if (other.CompareTag("Cell"))
            transform.SetParent(other);
    }

    protected abstract void OnSnap(Cell other);
}