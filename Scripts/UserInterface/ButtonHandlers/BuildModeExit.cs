using Godot;
using System;

public partial class BuildModeExit : Button
{
    public override void _Ready()
    {
        Pressed += Exit;
    }

    public void Exit()
    {
        BuildingManager.Instance.ExitBuildMode(true);
    }
}
