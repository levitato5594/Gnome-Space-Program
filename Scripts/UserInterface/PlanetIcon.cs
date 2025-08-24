using Godot;
using System;

public partial class PlanetIcon : TextureButton
{
    [Export] public CelestialBody planet;
    [Export] public Camera3D camera; // Doesn't matter if it's the scaled or local camera

    public override void _PhysicsProcess(double delta)
    {
        if (planet != null && camera != null) 
        {
            Vector3 planetPos = planet.GlobalPosition;
            Vector2 screenPos = camera.UnprojectPosition(planetPos);

            Position = screenPos;
        }
    }

    public override void _Pressed()
    {
        FlightCamera flightCam = FlightCamera.Instance;
        flightCam.target = planet;
        flightCam.Position = Vector3.Zero;

        flightCam.minZoom = (float)(planet.radius * 1.25f);
        flightCam.zoom = (float)(planet.radius * 2f);
    }
}
