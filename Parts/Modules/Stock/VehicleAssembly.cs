using Godot;
using Godot.Collections;
using System;

/*
    This part module is BUILT IN to GSP. 
    A couple of internal classes (BuildingManager for example) unfortunately rely on this.
    As such, this module should remain internal and not be compiled as a mod.
*/
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

        Array<float> initialPosArray = (Array<float>)configData["nodePosition"];
        Vector3 initialPos = new(
            initialPosArray[0],
            initialPosArray[1],
            initialPosArray[2]);

        maxZoom = (float)configData["maxZoom"];
        targetZoom = (float)configData["targetZoom"];

        // Create our stuff
        vab = new(){
            Position = initialPos
        };
        part.AddChild(vab);
        camPivot = new(){
            Position = Vector3.Zero
        };
        vab.AddChild(camPivot);

        PartMenuHandler.CreateButton(part.contextMenu.itemList, "Enter").Pressed += EnterFacility;

    }

    public void EnterFacility()
    {
        BuildingManager.Instance.SetVAB(this);
        BuildingManager.Instance.EnterBuildMode(camPivot, maxZoom, targetZoom, true);
    }
}
