using Godot;
using Godot.Collections;
using System;

// Registry of singletons for GDScript that otherwise couldn't be accessed.
public partial class SingletonRegistry : Node
{
    // Has to be non-static for GDScript
    public Dictionary registry = [];
    public static SingletonRegistry Instance { get; private set; }

    public override void _EnterTree()
    {
        Instance = this;
    }

    // Add to the great registry or not depends if it's already there
    public static void Register(Node singleton)
    {
        if(!Instance.registry.ContainsKey(singleton.Name)) Instance.registry.Add(singleton.Name, singleton);
    }
}
