using Godot;
using System;

public partial class BuildModeLaunch : Button
{
    // should have the launchsite module
    public Part launchsite;
    public override void _Ready()
    {
        Pressed += Launch;
    }

    public void Launch()
    {
        BuildingManager.Instance.LaunchCraft();
    }
}
