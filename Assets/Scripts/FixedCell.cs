using UnityEngine;

public class FixedCell : Cell
{
    protected override void OnSnap(Cell other)
    {
        FixedJoint2D joint = gameObject.AddComponent<FixedJoint2D>();
        joint.connectedBody = other.GetComponent<Rigidbody2D>();
    }
}
