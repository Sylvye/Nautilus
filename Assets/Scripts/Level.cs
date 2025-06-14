using UnityEngine;

public class Level : MonoBehaviour
{
    public BoxCollider2D boundsCol;
    public Vector2 size;

    void Awake()
    {
        boundsCol = GetComponent<BoxCollider2D>();
    }

    // will only collide with player layer
    private void OnTriggerEnter2D(Collider2D collision)
    {
        MapController.main.GenerateNextLevel();
        boundsCol.enabled = false;
    }
}
