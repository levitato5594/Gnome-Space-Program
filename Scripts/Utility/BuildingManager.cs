using Godot;
using System;

public partial class BuildingManager : Node
{
    public static readonly string classTag = "([color=Turquoise]BuildingManager[color=white])";
	public static BuildingManager Instance { get; private set; }

    [Export] public BuildUI buildUI;

    public enum EditorMode
    {
        None,
        Static,
        Dynamic
    }

    // None (0), Static (1), or Dynamic (2)
    public int editorMode = (int)EditorMode.None;
    // Whether or not other crafts or colonies are included
    // This should be true for static editing (in a VAB / editing colonies), and false for dynamic editing (EVA construction)
    public bool excludeOtherThings; 
    public Node3D editedThing;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        Instance = this;
		SingletonRegistry.Register(this); // Register self
    }

    public void OpenBuildUI(bool open, bool selector = true)
    {
        buildUI.Visible = open;
        buildUI.partListContainer.Visible = open && selector;
        if (open)
        {
            buildUI.partList.LoadPartList();
        }
    }
}
