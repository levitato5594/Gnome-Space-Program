using Godot;
using System;

public partial class VehicleAssembly : PartModule
{
    [Export] public Node3D camPivot;
    [Export] public float maxZoom = 35;
    [Export] public float targetZoom = 1;

    public override void PartInit() 
    {
        part.AddButton("Enter", "Enter");

        // TODO: Add methods to create buttons and clean up code
    }
}
