using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Cell : MonoBehaviour
{
    public bool locked;
    public int connections = 0;
    public int maxConnections = 3;

    public float snapRadius;
    public float lockedRadius;

    public GameObject connector;
    private Transform connectorParent;

    private HashSet<GameObject> bannedCells;
    private List<Cell> connected;

    private Rigidbody2D rb;
    private CircleCollider2D col;
    private LayerMask cellLayerMask;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CircleCollider2D>();
        cellLayerMask = LayerMask.GetMask("Cell");
        bannedCells = new HashSet<GameObject>();
        connected = new List<Cell>();
        connectorParent = GameObject.Find("Connectors").transform;
    }

    private void Start()
    {
        bannedCells.Add(gameObject);
    }

    void FixedUpdate()
    {
        // each cell should have a hashset of banned cells it should know not to connect to

        if (!locked)
        {
            RaycastHit2D[] hit = Physics2D.CircleCastAll(transform.position, snapRadius, Vector2.zero, 0, cellLayerMask);
            foreach (RaycastHit2D h in hit)
            {
                if (h.collider != null && !bannedCells.Contains(h.transform.gameObject) && Mathf.Abs(Vector2.Distance(transform.position, h.transform.position) - snapRadius) < 0.1f) // ensures it's the right distance away from the other cell
                {
                    Cell other = h.transform.gameObject.GetComponent<Cell>();

                    if (other.connections >= other.maxConnections)
                    {
                        bannedCells.Add(other.gameObject);
                        continue;
                    }

                    connected.Add(other);

                    Lock(other.transform);

                    Vector2 dir = (other.transform.position - transform.position).normalized;
                    Vector2 snapPosition = (Vector2)other.transform.position - dir * Mathf.Max(snapRadius, other.snapRadius);
                    transform.position = snapPosition;

                    FixedJoint2D joint = gameObject.AddComponent<FixedJoint2D>();
                    joint.connectedBody = other.GetComponent<Rigidbody2D>();

                    GameObject connObj = Instantiate(connector, connectorParent);
                    CellConnector cConn = connObj.GetComponent<CellConnector>();
                    cConn.c1 = gameObject;
                    cConn.c2 = other.gameObject;

                    connections++;
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!locked && collision.gameObject.layer == LayerMask.NameToLayer("Terrain"))
        {
            Lock(collision.transform);
            FixedJoint2D joint = gameObject.AddComponent<FixedJoint2D>();
            joint.connectedBody = collision.transform.GetComponent<Rigidbody2D>();
            joint.frequency = 20;
        }
    }

    private void Lock(Transform other)
    {
        locked = true;
        col.radius = lockedRadius;
        if (transform.parent == null && other.CompareTag("Cell"))
            transform.SetParent(other);
    }
}