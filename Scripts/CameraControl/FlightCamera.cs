using Godot;
using System;

public partial class FlightCamera : Node3D
{
	// THERE SHOULD ONLY EVER BE ONE FLIGHT CAMERA!!
	// The same camera (THIS ONE) is used in both map view, colony view, and flight
	public static readonly string classTag = "([color=MEDIUM_SPRING_GREEN]FlightCamera[color=white])";
	public static FlightCamera Instance { get; private set; }

    [Export] public bool inMap = true;
    [Export] public Key mapKey = Key.M;
    [Export] public Node3D target;

    [Export] public bool multiplyScroll;
	[Export] public float lerpSpeed = 1.0f;
	[Export] public float rotationAmnt = 1.0f;

	[Export] public Node3D rotNode_Y;
    [Export] public Node3D rotNode_X;
    [Export] public Node3D camNode;

	[Export] public float minZoom;
    [Export] public float maxZoom = float.PositiveInfinity;
    [Export] public float zoomAmnt;
    [Export] public float zoom;

    [Export] public Node3D facingDownObject;

    private Vector3 rotTarget_Y;
	private Vector3 rotTarget_X;

	private bool camRotating;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        Instance = this;
        rotTarget_Y = rotNode_Y.RotationDegrees;
		rotTarget_X = rotNode_X.RotationDegrees;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        float lerpy = lerpSpeed * (float)delta;

		rotNode_Y.RotationDegrees = rotNode_Y.RotationDegrees.Lerp(rotTarget_Y, lerpy);
        rotNode_X.RotationDegrees = rotNode_X.RotationDegrees.Lerp(rotTarget_X, lerpy);

		if (zoom > maxZoom) zoom = maxZoom;
		if (zoom < minZoom) zoom = minZoom;

        camNode.Position = camNode.Position.Lerp(new Vector3(0,0,zoom), lerpy);

        if (target != null) Position = target.GlobalPosition;

        if (facingDownObject != null && !inMap)
        {
            LookAt(facingDownObject.GlobalPosition, Vector3.Up);
            Rotate(Vector3.Right, 1.570795f);
        }else{
            GlobalRotation = new Vector3(0, 0, 0);
        }
    }

	public void TargetObject(Node3D node, float tgtMinZoom, float tgtZoom)
	{
		target = node;
        Position = Vector3.Zero;

        Logger.Print($"{classTag} Targeting node: {node.Name}");
        Logger.Print($"{classTag} Node scale: {node.Scale}");

        minZoom = tgtMinZoom; //node.Scale.Z * 1.25f / ScaledSpace.Instance.scaleFactor;
        zoom = tgtZoom; //node.Scale.Z * 2f / ScaledSpace.Instance.scaleFactor;
    }

	public void TargetObject(CelestialBody cBody)
	{
		target = cBody.scaledSphere;
        Position = Vector3.Zero;

		Logger.Print($"{classTag} Targeting cBody: {cBody.Name}");
        Logger.Print($"{classTag} cBody radius: {cBody.radius}");

        minZoom = (float)(cBody.radius * 1.25f / ScaledSpace.Instance.scaleFactor);
        zoom = (float)(cBody.radius * 2f / ScaledSpace.Instance.scaleFactor);
	}

	// Map view
	public void ToggleMapView(bool toggle)
	{
		Node3D thing = ActiveSave.Instance.activeThing;

        Logger.Print($"{classTag} Map view: {toggle}");
        inMap = toggle;
        ActiveSave.Instance.localSpace.Visible = !toggle;
		
		if (toggle)
		{
			// Implement craft too
            if (thing is Colony colony)
			{
                TargetObject(colony.scaledObject, 1, 2);
            }else
			{
				TargetObject(ActiveSave.Instance.activePlanet);
			}
        }else if (thing != null){
			// Might spell trouble but idc anymore
            TargetObject(thing, 5, 25);
        }
    }

    public override void _UnhandledInput(InputEvent @event)
    {
		// Keybinds
		if (@event is InputEventKey keyEvent)
		{
			// Open map and only open if active thing isn't null (otherwise don't allow it)
			if (keyEvent.Keycode == mapKey && keyEvent.Pressed && ActiveSave.Instance.activeThing != null)
			{
                ToggleMapView(!inMap);
            }
		}

        if (@event is InputEventMouseButton buttonEvent)
		{
			switch (buttonEvent.ButtonIndex)
			{
				case MouseButton.Right:
					camRotating = buttonEvent.Pressed;
					break;
				case MouseButton.WheelUp:
					if(multiplyScroll)
					{
						zoom /= zoomAmnt;
					}else{
						zoom -= zoomAmnt;
					}
					break;
				case MouseButton.WheelDown:
					if(multiplyScroll)
					{
						zoom *= zoomAmnt;
					}else{
						zoom += zoomAmnt;
					}
					break;
			}
		}
		if (@event is InputEventMouseMotion motion && camRotating == true)
		{
			rotTarget_Y += Vector3.Up * -motion.Relative.X*rotationAmnt;
			rotTarget_X += Vector3.Right * -motion.Relative.Y*rotationAmnt;
		}
    }
}
