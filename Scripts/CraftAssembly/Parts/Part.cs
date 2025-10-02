using Godot;
using System;
using System.Collections.Generic;

/* 
Technically this class encompasses both colony AND ship parts, as I intend for them to be used interchangeably.
Why? Because I want players to have the freedom to get up to any sort of shenanigans with these systems.
*/
public partial class Part : RigidBody3D
{
    [Export] public bool enabled = false;
    [Export] public Material glowMat;
    [Export] public MeshInstance3D glowMesh;
    [Export] public bool selectable = true;

    // Whether or not to treat the part as "not real" (in the editor)
    [Export] public bool inEditor;

    [Signal] public delegate void SendButtonEventHandler(string name);

    public CachedPart cachedPart;
    public Node3D parentThing;

    public List<Node> partModules = [];
    // Name, category
    public Godot.Collections.Dictionary buttons = [];

    public override void _Process(double delta)
    {
        Highlight(PartMenuHandler.Instance.hoveredPart == this);
    }

    public void InitPart()
    {
        Logger.Print($"(Instance {Name}) Getting part modules...");
        partModules = GetModules(this);
        Logger.Print($"(Instance {Name}) Got all part modules! Count: {partModules.Count}");
        InitModules();
    }

    // Recursive function to find every part module GAAHH DAMN CROSS LANGUAGE SCRIPTING SUCKS
    public List<Node> GetModules(Node parent)
    {
        List<Node> modules = [];
        foreach (Node node in parent.GetChildren())
        {
            GDScript script = (GDScript)node.GetScript();
            
            if (script != null)
            {
                string id = (string)node.Get("identification");
                if (id == "PartModule")
                {
                    node.Set("part_node", this);
                    modules.Add(node);
                }
            }
            if(node.GetChildCount() > 0) modules.AddRange(GetModules(node));
        }
        return modules;
    }

    // Start it all up
    public void InitModules()
    {
        foreach (Node node in partModules)
        {
            node.Call("part_init");
        }
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
}
