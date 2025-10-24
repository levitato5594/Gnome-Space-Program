using Godot;
using System;
using System.Collections.Generic;

public partial class Craft : RigidBody3D
{
    public List<SavedPart> savedParts;
    public List<Part> loadedParts;

    // Hiujjj??
    public void Instantiate()
    {
        Instantiate(savedParts);
    }

    // I don't wants parts to be individually simulated so we mangle the shit out of this physics engine
    public void Instantiate(List<SavedPart> parts)
    {
        foreach (SavedPart savedPart in parts)
        {
            Part part = savedPart.reference.Instantiate(this);
            Godot.Collections.Dictionary partData = savedPart.partData;
            part.Position = (Vector3)partData["position"];
            part.Rotation = (Vector3)partData["rotation"];

            loadedParts.Add(part);
        }
    }
}