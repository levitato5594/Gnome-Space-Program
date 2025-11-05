using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

/* 
Technically this class encompasses both colony AND ship parts, as I intend for them to be used interchangeably.
Why? Because I want players to have the freedom to get up to any sort of shenanigans with these systems.
*/
public partial class Part : Area3D
{
    [Export] public bool enabled = false;
    [Export] public Material glowMat;
    [Export] public MeshInstance3D glowMesh;
    [Export] public bool selectable = true;

    // Whether or not to treat the part as "not real" (in the editor)
    [Export] public bool inEditor;

    [Export] public Array<AttachNode> attachNodes;

    // What to copy over to the craft upon intantiation
    [Export] public Array<CollisionShape3D> colliders;

    [Signal] public delegate void SendButtonEventHandler(string name);

    public CachedPart cachedPart;
    public Node3D parentThing;

    public List<PartModule> partModules = [];
    // Name, category
    public Dictionary buttons = [];

    // Assigned by stufffy like the part picker in the editor
    public long id;

    public override void _Process(double delta)
    {
        if (PartMenuHandler.Instance != null)
        {
            Highlight(PartManager.Instance.hoveredPart == this); 
        }
    }

    public void InitPart()
    {
        //Dictionary moduleData = (Dictionary)cachedPart.config["modules"];
        Logger.Print($"(Instance {Name}) Getting part modules...");
        //partModules = GetModules(this);
        Logger.Print($"(Instance {Name}) Got all part modules! Count: {partModules.Count}");
        //InitModules();
    }
    
    public List<PartModule> CreateModules(Dictionary data)
    {
        List<PartModule> modules = [];
        
        foreach (KeyValuePair<Variant,Variant> pair in data)
        {
            if (pair.Value.VariantType == Variant.Type.Dictionary)
            {
                string moduleName = (string)pair.Key;
            }
        }

        return modules;
    }

    public void ReadData(Dictionary data)
    {
        Position = (Vector3)data["position"];
        RotationDegrees = (Vector3)data["rotation"];
        id = (int)data["partID"];

        Dictionary attachmentData = (Dictionary)data["attachments"];
        foreach (KeyValuePair<Variant, Variant> node in attachmentData)
        {
            // IMplement lateor
        }
    }

    public Dictionary GetData()
    {
        Dictionary data = [];

        // Throw basic info into here
        data.Add("position", Position);
        data.Add("rotation", RotationDegrees);
        data.Add("partID", id);

        // Index - Index of attachment node HERE
        // Key - ID of the other part
        // Value - Index of the OTHER attachment node
        Dictionary attachmentData = [];
        foreach (AttachNode node in attachNodes)
        {
            if (node.connectedNode != null)
            {
                attachmentData.Add(node.connectedNode.part.id, node.connectedNode.part.attachNodes.IndexOf(node.connectedNode));
            }
        }

        data.Add("attachments", attachmentData);

        // Fetch data from every part module
        // IMPLEMENT C# TOO SOMEDAY pppwwweeaaaaaseeeeeee 
        Dictionary moduleDataContainer = [];
        //foreach (Node module in partModules)
        //{
        //    Dictionary moduleData = (Dictionary)module.Call("getData");
        //    moduleDataContainer.Add(module.GetScript(), moduleData);
        //}

        data.Add("modules", moduleDataContainer);

        return data;
    }

    public void AddButton(string category, string name)
    {
        Logger.Print($"(Instance {Name}) Adding button {name}");
        buttons.Add(category, name);
    }

    public void Highlight(bool toggle)
    {
        if (glowMesh != null)
        {
            if (toggle)
            {
                glowMesh.MaterialOverlay = glowMat;
            }else{
                glowMesh.MaterialOverlay = null;
            }
        }
    }

    // For C# -> GDScript scripting
    public void SendButtonEvent(string name)
    {
        Logger.Print($"(Instance {Name}) Sending button signal {name}");
        EmitSignal(SignalName.SendButton, name);
    }

    // Recursive function to get all meshes
    public List<MeshInstance3D> GetMeshes(Node node = null)
    {
        node ??= this;

        List<MeshInstance3D> meshList = [];

        foreach(Node child in node.GetChildren())
        {
            if (child is MeshInstance3D mesh)
            {
                meshList.Add(mesh);
            }

            if (child.GetChildCount() > 0)
            {
                List<MeshInstance3D> meshBuffer = GetMeshes(child);
                meshList.AddRange(meshBuffer);
            }
        }

        return meshList;
    }

    public Aabb GetAABB()
    {
        List<MeshInstance3D> meshList = GetMeshes(this);

        Aabb aabb = meshList[0].GetAabb();

        foreach (MeshInstance3D mesh in meshList)
        {
            //Logger.Print($"{Name} {mesh.GetAabb()}");
            aabb.Merge(mesh.GetAabb());
        }

        return aabb;
    }
}
