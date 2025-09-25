using Godot;
using System;

/* 
Everything "reality-breaking" should ideally be done here.

That includes stuff like floating origin, inverse rotation, velocity frames, all that BS.
Exceptions may include ScaledSpace and the OrbitManager 
(Stuff within OrbitManager might need to be moved here..? or vice-versa depends on how angry I am)
*/
public partial class RealityTangler : Node
{
    public static RealityTangler Instance { get; private set; }

    [Export] public float originResetThreshold = 100;
    public Double3 originOffset = Double3.Zero;

    public override void _Ready()
    {
        Instance = this;
    }

    // Lotsa EVIL stuff
    public override void _PhysicsProcess(double delta)
    {
        Node3D activeThing = ActiveSave.Instance.activeThing;
        if (activeThing != null)
        {
            // We don't need the square root of this anyways
            float originDistance = activeThing.GlobalPosition.DistanceSquaredTo(Vector3.Zero);

            if (originDistance > originResetThreshold * originResetThreshold)
            {
                ResetOrigin(activeThing);
            }
        }
    }

    // Resets origin. Duh.
    public void ResetOrigin(Node3D relativeTo)
    {
        Vector3 focusedObjectPos = relativeTo.GlobalPosition;
        //focusedObject.GlobalPosition -= focusedObjectPos;

        originOffset -= Double3.ConvertToDouble3(focusedObjectPos);

        GD.Print(originOffset);
    }
}
