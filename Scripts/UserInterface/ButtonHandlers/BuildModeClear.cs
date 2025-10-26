using Godot;
using System;

public partial class BuildModeClear : Button
{
    public override void _Ready()
    {
        Pressed += Clear;
    }

    public void Clear()
    {
        BuildingManager.Instance.ClearParts();
    }
}
