using UnityEngine;
using UnityEngine.Tilemaps;

public class Level : MonoBehaviour
{
    public BoxCollider2D boundsCol;
    public Vector2 size;

    void Awake()
    {
        boundsCol = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        // i hate doing this but its the only way I have found that correctly creates collider shapes at runtime
        TilemapCollider2D tc = gameObject.AddComponent<TilemapCollider2D>();
        tc.compositeOperation = Collider2D.CompositeOperation.Merge;
        CompositeCollider2D cc = gameObject.AddComponent<CompositeCollider2D>();
        cc.geometryType = CompositeCollider2D.GeometryType.Polygons;
    }

    // will only collide with player layer
    private void OnTriggerEnter2D(Collider2D collision)
    {
        MapController.main.GenerateNextLevel();
        boundsCol.enabled = false;
    }
}
