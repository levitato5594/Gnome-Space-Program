using Godot;
using System;
using System.Collections.Generic;

public partial class Craft : RigidBody3D
{
    public List<Part> currentParts;

    // I don't wants parts to be individually simulated so we mangle the shit out of this physics engine
    public void Instantiate(List<SavedPart> parts)
    {
        foreach (SavedPart savedPart in parts)
        {
            Part part = savedPart.reference.Instantiate(this);
            part.Position = savedPart.position;
            part.Rotation = savedPart.rotation;
        }
    }
}