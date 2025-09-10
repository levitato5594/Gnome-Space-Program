using Godot;
using System;

public partial class PlanetIcon : MapIcon
{
    [Export] public CelestialBody planet;

    public override void _Ready()
    {
        thing = planet.scaledSphere;
    }

    public override void _Pressed()
    {
        FlightCamera flightCam = FlightCamera.Instance;
        flightCam.TargetObject(planet);
    }
}
