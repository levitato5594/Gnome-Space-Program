using Godot;
using System.Collections.Generic;

public partial class Colony : Node3D
{
    /*
    Colonies also use "parts" to take advantage of the attachment node system.
    In return, craft get to take advantage of stuff like launchsites and VABs.
    */

    public CelestialBody parentBody;
    public ScaledObject scaledObject;

    public string name;
    public Double3 position;
    public Double3 rotation;

    public Dictionary<string, UnloadedPart> savedRootParts = []; // Only lists the saved root parts
    public Dictionary<string, UnloadedPart> savedParts = [];

    public List<Part> rootParts;
    public List<Part> allParts;

    public void Load()
    {
        // Iterate over parts
        GD.Print($"({name}) Instantiating...");
        foreach (KeyValuePair<string, UnloadedPart> pair in savedRootParts)
        {
            string partName = pair.Key;
            UnloadedPart data = pair.Value;

            // ... Now we need to make a part manager.....
            // ... Okay now that we made a part manager.....

            Part part = data.template.Instantiate(this);
            part.Freeze = true;
            part.LockRotation = true;
            part.Position = data.position;
            part.Rotation = data.rotation;
        }
    }

    public void Unload()
    {
        
    }
}