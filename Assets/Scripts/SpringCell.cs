using UnityEngine;

public class SpringCell : Cell
{
    protected override void OnSnap(Cell other)
    {
        SpringJoint2D joint = gameObject.AddComponent<SpringJoint2D>();
        joint.connectedBody = other.GetComponent<Rigidbody2D>();
        joint.autoConfigureConnectedAnchor = true;
        joint.enableCollision = true;
    }
}
