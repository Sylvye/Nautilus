using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapController : MonoBehaviour
{
    public static MapController main;
    public List<Level> levels;
    public List<float> weights;
    public Level current;
    public  int levelCount;
    private List<GameObject> currentLevels;
    private Vector2 currentPos;
    private Transform grid;

    void Awake()
    {
        main = this;
        currentPos = Vector2.up * 5;
        grid = GameObject.Find("Grid").transform;
        currentLevels = new List<GameObject>();
    }

    private void Start()
    {
        GenerateNextLevel();
    }

    public void GenerateNextLevel()
    {
        int r = WeightedRandom.SelectWeightedIndex(weights);
        GameObject lObj = Instantiate(levels[r].gameObject, currentPos, Quaternion.identity);
        Level l = lObj.GetComponent<Level>();
        lObj.transform.parent = grid;

        currentPos += Vector2.up * l.size.y;
        currentLevels.Add(lObj);
        current = l;
        levelCount++;

        if (levelCount > 3)
            DestroyOldestLevel();
    }

    public void DestroyOldestLevel()
    {
        GameObject lastLevel = currentLevels[0];
        currentLevels.Remove(lastLevel);
        Destroy(lastLevel);
        levelCount--;
    }
}
