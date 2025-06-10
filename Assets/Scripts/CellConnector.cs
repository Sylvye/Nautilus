using System.Collections.Generic;
using UnityEngine;

public class CellConnector : MonoBehaviour
{
    public GameObject c1;
    public GameObject c2;
    public bool solid = false;

    private LineRenderer lr;
    private EdgeCollider2D ec;

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
        ec = GetComponent<EdgeCollider2D>();
    }

    void Start()
    {
        SetSolid(solid);
        transform.position = Vector3.forward * 3;
    }

    void Update()
    {
        if (c1 == null || c2 == null)
        {
            Destroy(gameObject);
            return;
        }
        lr.SetPositions(new Vector3[] { c1.transform.position, c2.transform.position });
        ec.SetPoints(new List<Vector2> { c1.transform.position, c2.transform.position });
    }

    public void SetSolid(bool s)
    {
        solid = s;
        ec.enabled = solid;
    }
}
