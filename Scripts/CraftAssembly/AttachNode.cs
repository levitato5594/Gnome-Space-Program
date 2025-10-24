using Godot;
using System;

public partial class AttachNode : Node3D
{
	[Export] public bool canRecieve = true;

	// Parent part
    [Export] public Part part;

    // The other part's node.
    public AttachNode connectedNode;

	public void Attach(AttachNode otherNode)
	{
        connectedNode = otherNode;
        otherNode.connectedNode = this;
    }

	public void Detach()
	{
		if (connectedNode != null)
		{
			connectedNode.connectedNode = null;
    		connectedNode = null;
		}
	}
}
