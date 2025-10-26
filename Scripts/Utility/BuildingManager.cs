using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public partial class BuildingManager : Node
{
    public static readonly string classTag = "([color=Turquoise]BuildingManager[color=white])";
	public static BuildingManager Instance { get; private set; }

    // Current active VAB.
    public Node3D activeVAB;
    // Where to instantiate parts
    [Export] public Node3D editorPartContainer;
    [Export] public Node3D floatingPartContainer;
    [Export] public BuildUI buildUI;
    [Export] public float partMoveSpeed = 15f;
    [Export] public float partHoldDistance = 5f;
    [Export] public float partSnapDistance = 30f;
    [Export] public bool verboseLogging;

    public enum EditorMode
    {
        None,
        Static,
        Dynamic
    }

    // None (0), Static (1), or Dynamic (2)
    public int editorMode = (int)EditorMode.None;
    
    // Guess what, it's the part being dragged!
    public Part draggingPart;
    public (AttachNode, AttachNode) snappedNodes;
    public List<Part> partsList;
    // We orient around this one
    public Part centralPart;

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

            (AttachNode, AttachNode) attachNodeBuffer = (null, null);

            // Part Snappy
            foreach (Part part in partsList)
            {
                if (part != draggingPart) // Redundant but who tf cares
                {
                    foreach (AttachNode attachNode0 in part.attachNodes)
                    {
                        if (attachNode0.connectedNode == null)
                        {
                            // Iterate over the dragged part's nodes

                            foreach (AttachNode attachNode1 in draggingPart.attachNodes)
                            {
                                // We get screen positions to avoid depth problems
                                Camera3D localCam = ActiveSave.Instance.localCamera;

                                Vector3 node0Pos = attachNode0.GlobalPosition;
                                // Imagine this one (relative node position + supposed global position)
                                Vector3 node1Pos = attachNode1.GlobalPosition - draggingPart.GlobalPosition + projectedPosition;

                                Vector2 nodeScreenPos0 = localCam.UnprojectPosition(node0Pos);
                                Vector2 nodeScreenPos1 = localCam.UnprojectPosition(node1Pos);

                                float distance = nodeScreenPos0.DistanceSquaredTo(nodeScreenPos1);

                                if (distance <= partSnapDistance*partSnapDistance)
                                {
                                    // Override the part's position with a new one !!
                                    projectedPosition = attachNode0.GlobalPosition - (attachNode1.GlobalPosition - draggingPart.GlobalPosition);
                                    // We remember those two nodes if we ever want to place the part again
                                    attachNodeBuffer = (attachNode0, attachNode1);
                                    break; // Exit the loop
                                }
                            }
                        }
                    }
                }
            }

            // We apply! !! !!! ! 1
            draggingPart.GlobalPosition = draggingPart.GlobalPosition.Lerp(projectedPosition, partMoveSpeed*(float)delta);
            snappedNodes = attachNodeBuffer;
        }

        Godot.Collections.Array<Node> parts = editorPartContainer.GetChildren();

        // There's no concrete reason for a buffer but it makes some of this more manageable maybe?? uhhm uhh err :3
        List<Part> partListBuffer = [];
        foreach (Node node in parts)
        {
            if (node is Part part)
            {
                partListBuffer.Add(part);
            }
        }
        partsList = partListBuffer;

        // Just pick one if it's null (the user will take control, otherwise)
        if (partListBuffer.Count > 0 && centralPart == null)
        {
            centralPart = partListBuffer[0];
            Logger.Print($"{classTag} Auto assigned central part to: {centralPart.Name}");
            centralPart.Position = new Vector3(0, 0, 0);
        }
    }

    public void LaunchCraft()
    {
        Dictionary partData = PartManager.CompilePartData(partsList);

        if (verboseLogging)
            Logger.Print(partData);

        Craft craft = new();
        ActiveSave.Instance.localSpace.AddChild(craft);
        craft.Instantiate(partData);

        craft.GlobalPosition = editorPartContainer.GlobalPosition;
    }

    public void ClearParts()
    {
        foreach (Part part in partsList)
        {
            part.QueueFree();
        }

        centralPart = null;
        partsList = [];
    }

    public void SetVAB(Node3D vab)
    {
        activeVAB = vab;
        editorPartContainer.GlobalTransform = vab.GlobalTransform;
        floatingPartContainer.GlobalTransform = vab.GlobalTransform;
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
                    draggingPart.Reparent(floatingPartContainer);

                    // Fully detach, since we're taking the part off of others.
                    foreach (AttachNode attachNode in draggingPart.attachNodes)
                    {
                        attachNode.Detach();
                    }

                    Logger.Print($"{classTag} Selected part {draggingPart.Name}");
                }else if (draggingPart != null) {
                    draggingPart.Reparent(editorPartContainer);

                    if (snappedNodes.Item1 != null && snappedNodes.Item2 != null)
                        snappedNodes.Item1.Attach(snappedNodes.Item2);

                    Logger.Print($"{classTag} Unselected part {draggingPart.Name}");
                    draggingPart = null;
                }
            }
        }
    }
}
