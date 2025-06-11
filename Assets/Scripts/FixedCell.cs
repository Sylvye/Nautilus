using UnityEngine;

public class FixedCell : Cell
{
    protected override void OnSnap(Cell other)
    {
        FixedJoint2D joint = gameObject.AddComponent<FixedJoint2D>();
        joint.connectedBody = other.GetComponent<Rigidbody2D>();
        joint.autoConfigureConnectedAnchor = true;
        joint.enableCollision = true;
        joint.dampingRatio = 0.7f;
        joint.frequency = 2;
    }
}
