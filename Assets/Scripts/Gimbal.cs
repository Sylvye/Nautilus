using UnityEditor.Timeline;
using UnityEngine;

public class Gimbal : MonoBehaviour
{
    void LateUpdate()
    {
        transform.rotation = Quaternion.identity;
    }
}
