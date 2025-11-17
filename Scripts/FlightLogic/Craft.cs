using Godot;
using Godot.Collections;
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
    public void Instantiate(Dictionary partData)
    {
        RealityTangler.Instance.OriginReset += ResetOrigin;
        this.partData = partData;
        foreach (KeyValuePair<Variant, Variant> data in partData)
        {
            if (data.Key.VariantType == Variant.Type.Int)
            {
                string partName = (string)((Dictionary)data.Value)["name"];
                Dictionary theActualFuckingData = (Dictionary)((Dictionary)data.Value)["data"];

                CachedPart cachedPart = PartManager.Instance.partCache[partName];

                Part part = cachedPart.Instantiate(this, false, true);
                part.ReadData(theActualFuckingData);
            }
        }
    }

    // Middle-man function in case we want something special to happen
    public void SnatchFocus()
    {
        ActiveSave.Instance.activeThing = this;
        FlightCamera.Instance.TargetObject(this);
    }

    public void ResetOrigin()
    {
        if (ActiveSave.Instance.activeThing == this)
        {
            Logger.Print("RESET");
            GlobalPosition = Vector3.Zero;
        }
    }
}