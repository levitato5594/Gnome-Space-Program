using Godot;
using System;
using System.Collections.Generic;

public partial class CelestialBody : Node3D
{
    // General info
    public string name;
    public bool focusOnload;
    public double mass;
    public double geeASL;
    public double radius;
    public Vector3 originPos;

    // Orbital info
    public string parentName;
    public Orbit orbit;
    public CartesianData cartesianData;

    public List<CelestialBody> childPlanets = [];

    // Procedural info
    public TerrainGen pqsSphere;
    public ScaledObject scaledSphere;
    public List<Node> pqsMods;

    // Miscellaneous info
    public bool isRoot; // only ONE body per save should ever have this be true!
    public string configPath;

    // DEBUG
    public MeshInstance3D debugOrb;

    // Functions to get points with Y as up rather than Z
    // To Be Eliminated
    private static Vector3 GetPosYUp(Vector3 inputVector)
    {
        return new Vector3(inputVector.X,inputVector.Z,inputVector.Y);
    }

    public void CreateDebugOrb(Node3D parent)
    {
        debugOrb = new MeshInstance3D();
        debugOrb.Mesh = new SphereMesh();
        parent.AddChild(debugOrb);
    }

    public override void _Process(double delta)
    {   
        // Propagate the cBody's orbit

        //ProcessOrbitalPosition();

        //scaledSphere.truePosition = GetPosYUp(cartesianData.position);
    }

    // Process the cBody orbital positioning calculations. Used by RealityTangler to "force" repositioning to avoid jitter.
    public void ProcessOrbitalPosition()
    {
        if (orbit != null)
        {
            orbit.trueAnomaly = PatchedConics.TimeToTrueAnomaly(orbit, ActiveSave.Instance.saveTime, 0) + orbit.trueAnomalyAtEpoch;
            (Vector3 position, Vector3 velocity) = PatchedConics.KOEtoECI(orbit);
            cartesianData.position = position + orbit.parent.cartesianData.position;
            cartesianData.velocity = velocity;
            //GD.Print(SaveManager.Instance.saveTime);
            //GD.Print($"{cartesianData.position.X}, {cartesianData.position.Y}, {cartesianData.position.Z}");
        }

        // Uh
        originPos = cartesianData.position + GetPosYUp(RealityTangler.Instance.originOffset);

        // Modify originPos such that the active planet is at at a the world origin
        if (ActiveSave.Instance.activePlanet != null)
            originPos -= ActiveSave.Instance.activePlanet.cartesianData.position;

        Position = GetPosYUp(originPos);

        scaledSphere.truePosition = GlobalPosition; //cBody.cartesianData.position.GetPosYUp();
        scaledSphere.ForceUpdate();
    }

    public void ResetOrigin()
    {
        // Just to prevent jitter
        ProcessOrbitalPosition();
    }

    public override string ToString()
    {
        return name;
    }
}
