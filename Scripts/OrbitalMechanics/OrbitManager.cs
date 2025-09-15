using Godot;
using System;

// Singleton that FORCES orbits to propagate
public partial class OrbitManager : Node
{
    public override void _PhysicsProcess(double delta)
    {
        foreach (CelestialBody cBody in PlanetSystem.Instance.celestialBodies)
        {
            // Force many things to update because fuck I don't know
            ScaledSpace.Instance.ForceUpdate();

            cBody.ProcessOrbitalPosition();

            cBody.scaledSphere.truePosition = cBody.cartesianData.position.GetPosYUp();
            cBody.scaledSphere.ForceUpdate();
        }
    }
}
