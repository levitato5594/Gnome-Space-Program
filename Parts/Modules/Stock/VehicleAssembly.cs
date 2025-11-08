using Godot;
using Godot.Collections;
using System;

public partial class VehicleAssembly : PartModule
{
    public Node3D vab;
    public Node3D camPivot;
    public float maxZoom = 35;
    public float targetZoom = 1;

    public override void PartInit() 
    {
        // Gather data
        bool verticalScroll = (bool)configData["verticalScroll"];

        Array<float> initialPosArray = (Array<float>)configData["pivotInitialPos"];
        Vector3 pivotInitialPos = new(
            initialPosArray[0],
            initialPosArray[1],
            initialPosArray[2]);

        bool maxZoom = (bool)configData["maxZoom"];
        bool targetZoom = (bool)configData["targetZoom"];

        // Create our stuff
        vab = new(){
            Position = new Vector3(0, 0, 0)
        };
        part.AddChild(vab);
        camPivot = new(){
            Position = pivotInitialPos
        };
        vab.AddChild(camPivot);

        PartMenuHandler.CreateButton(part.contextMenu.itemList, "Enter").Pressed += EnterFacility;

    }

    public void EnterFacility()
    {
        BuildingManager.Instance.EnterBuildMode(camPivot, maxZoom, targetZoom, true);
    }
}
