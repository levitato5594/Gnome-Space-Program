using Godot;
using System;
using System.Collections.Generic;

public partial class Craft : RigidBody3D
{
    public Godot.Collections.Dictionary partData;
    public List<Part> loadedParts;

    // Hiujjj??
    public void Instantiate()
    {
        Instantiate(partData);
    }

    // I don't wants parts to be individually simulated so we mangle the shit out of this physics engine
    public void Instantiate(Godot.Collections.Dictionary partData)
    {
        this.partData = partData;
        foreach (KeyValuePair<Variant, Variant> data in partData)
        {
            if (data.Key.VariantType == Variant.Type.String)
            {
                string partName = (string)data.Key;
                CachedPart cachedPart = PartManager.Instance.partCache[partName];

                Part part = cachedPart.Instantiate(this, false, true);
                part.ReadData((Godot.Collections.Dictionary)data.Value);
            }
        }
    }
}