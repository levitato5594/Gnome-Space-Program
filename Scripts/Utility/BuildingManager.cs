using Godot;
using System;

public partial class BuildingManager : Node
{
    public static readonly string classTag = "([color=Turquoise]BuildingManager[color=white])";
	public static BuildingManager Instance { get; private set; }

    // None, Craft, or Colony
    public string editorMode = "None";
    public Node3D editedThing;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        Instance = this;
		SingletonRegistry.Register(this); // Register self
    }
}
