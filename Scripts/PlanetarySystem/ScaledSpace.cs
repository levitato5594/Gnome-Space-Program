using Godot;
using System;

/*
Potentially add an "offset" position for when in map view so that scaled space can
move around the camera rather than the camera moving around it 
(also add override when in map view so that the camera can navigate around scaled space -
currently all it is doing is moving in REAL space which causes floating point imprecision bugs)

TL;DR the camera has to look at scaled space in map view, and have scaled space move instead of the camera.
*/
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
