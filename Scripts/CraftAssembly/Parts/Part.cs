using Godot;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;

/* 
BOOYAH!
Technically this class encompasses both colony AND ship parts, as I intend for them to be used interchangeably.
Why? Because I want players to have the freedom to get up to any sort of shenanigans with these systems.
*/
public partial class Part : RigidBody3D
{
    [Export] public bool enabled = false;

    public List<Node> partModules = [];

    public override void _Ready()
    {
        Logger.Print($"(Instance {Name}) Getting part modules...");
        partModules = GetPartModules(this);
        Logger.Print($"(Instance {Name}) Got all part modules! Count: {partModules.Count}");
    }

    // Recursive function to find every part module GAAHH DAMN CROSS LANGUAGE SCRIPTING SUCKS
    public List<Node> GetPartModules(Node parent)
    {
        List<Node> modules = [];
        foreach (Node node in parent.GetChildren())
        {
            GDScript script = (GDScript)node.GetScript();
            
            if (script != null)
            {
                string id = (string)node.Get("identification");
                if (id == "PartModule")modules.Add(node);
            }
            if(node.GetChildCount() > 0) modules.AddRange(GetPartModules(node));
        }
        return modules;
    }

    public void InitPart()
    {
        
    }
}
