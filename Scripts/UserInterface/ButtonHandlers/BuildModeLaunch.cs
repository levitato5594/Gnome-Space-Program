using Godot;
using System;

public partial class BuildModeLaunch : Button
{
    public override void _Ready()
    {
        Pressed += Launch;
    }

    public void Launch()
    {
        BuildingManager.Instance.LaunchCraft();
    }
}
