using UnityEditor;
using UnityEngine;

public class MapCameraController : MonoBehaviour
{
    public static MapCameraController main;

    private void Awake()
    {
        main = this;
    }
    private void Update()
    {
        transform.position = new Vector3(PlayerController.main.transform.position.x, PlayerController.main.transform.position.y, -100);
    }
}
