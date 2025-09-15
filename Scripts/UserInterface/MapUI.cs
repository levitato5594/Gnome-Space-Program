using Godot;
using System;

// Singleton for all the map ui bs
public partial class MapUI : Control
{
    public static MapUI Instance { get; private set; }

    [Export] public ContextMenus contextMenus;

    public override void _Ready()
    {
        Instance = this;
    }
}
