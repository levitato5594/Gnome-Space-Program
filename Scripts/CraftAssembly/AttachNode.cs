using Godot;
using System;

public partial class AttachNode : Node3D
{
	[Export] public bool canRecieve = true;

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
