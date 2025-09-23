using UnityEngine;

public class ConnectJointToParent : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Joint2D joint = GetComponent<Joint2D>();
        joint.connectedBody = transform.parent.GetComponent<Rigidbody2D>();
    }
}
