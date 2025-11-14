/*
using Godot;
using System;

// Singleton that FORCES orbits to propagate and manages celestial positiining gnug uig ruigh uiarhg
// OBSOLETE: look at RealityTangler.cs
public partial class OrbitManager : Node
{
    public override void _PhysicsProcess(double delta)
    {
        foreach (CelestialBody cBody in PlanetSystem.Instance.celestialBodies)
        {
            // Force many things to update because I don't know
            ScaledSpace.Instance.ForceUpdate();

            cBody.ProcessOrbitalPosition();
            cBody.scaledSphere.truePosition = Double3.ConvertToDouble3(cBody.GlobalPosition); //cBody.cartesianData.position.GetPosYUp();
            cBody.scaledSphere.ForceUpdate();
        }
    }
}
*/
