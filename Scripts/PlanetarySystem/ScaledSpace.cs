using Godot;
using System;

public partial class ScaledSpace : Node3D
{
    // Obligatory.
    public static ScaledSpace Instance { get; private set; }
    // By how much the scale is divided by
    [Export] public float scaleFactor = 10000;
    [Export] public FlightCamera flightCamera;

    public override void _Ready()
    {
        Instance = this;
    }

    public override void _Process(double delta)
    {
        Godot.Collections.Array<Node> childNodes = GetChildren();

        foreach (Node node in childNodes)
        {
            if (node is ScaledObject scaledObject)
            {
                scaledObject.Position = scaledObject.truePosition.ToFloat3();
            }
        }
    }
}
