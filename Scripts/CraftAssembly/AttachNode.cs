using Godot;
using System;

public partial class AttachNode : Node3D
{
	[Export] public bool canRecieve = true;

    // The other part's node.
    public AttachNode connectedNode;
}
