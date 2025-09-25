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

    public override void _PhysicsProcess(double delta)
    {
        ForceUpdate();
    }

    public void ForceUpdate()
    {
        Godot.Collections.Array<Node> childNodes = GetChildren();

        foreach (Node node in childNodes)
        {
            if (node is ScaledObject scaledObject)
            {
                // Handle ScaledSpace differently if in map view
                if (!flightCamera.inMap)
                {
                    // TODO: Position scaled space objects to visually align with local space
                    //throw new NotImplementedException();
                }else{
                    Node3D camObject = flightCamera.target;
                    Vector3 focusObjectPos = Vector3.Zero;
                    // Check if the camera is focusing on either a ScaledObject or another thing that isn't implemented yet
                    if (camObject is ScaledObject scaledCamObj)
                    {
                        focusObjectPos = scaledCamObj.truePosition.ToFloat3();
                    }
                
                    scaledObject.Position = scaledObject.truePosition.ToFloat3() / scaleFactor - (focusObjectPos / scaleFactor);
                    scaledObject.Scale = scaledObject.originalScale.ToFloat3() / scaleFactor;
                }
            }
        }
    }
}
