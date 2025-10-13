using Godot;
using System;

public partial class BuildingManager : Node
{
    public static readonly string classTag = "([color=Turquoise]BuildingManager[color=white])";
	public static BuildingManager Instance { get; private set; }

    // Current active VAB. Where parts are instantiated into
    public Node3D activeVAB;
    [Export] public BuildUI buildUI;
    [Export] public float partMoveSpeed = 5f;
    [Export] public float partHoldDistance = 5f;

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

    // Guess what, it's the part being dragged!
    public Part draggingPart;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        Instance = this;
		SingletonRegistry.Register(this); // Register self
    }

    public override void _Process(double delta)
    {
		if (draggingPart != null)
		{
            Camera3D camera = ActiveSave.Instance.localCamera;

			Vector2 mousePos = GetViewport().GetMousePosition();
			Vector3 projectedPosition = camera.ProjectPosition(mousePos, partHoldDistance);

			draggingPart.GlobalPosition = draggingPart.GlobalPosition.Lerp(projectedPosition, partMoveSpeed*(float)delta);
		}
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

    // Inputs !!!
    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseButt)
        {
            if (mouseButt.Pressed && mouseButt.ButtonIndex == MouseButton.Left)
            {
                if (draggingPart == null && PartManager.Instance.hoveredPart != null && PartManager.Instance.hoveredPart.inEditor) 
                {
                    draggingPart = PartManager.Instance.hoveredPart;
                    Logger.Print("Selecting part");
                }else{
                    draggingPart = null;
                    Logger.Print("Unselecting part");
                }
            }
        }
    }
}
