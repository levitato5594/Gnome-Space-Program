using Godot;
using Godot.Collections;
using System;

public partial class BuildModeLaunchMenu : Button
{
    [Export] public ContextMenus contextMenus;
    public override void _Ready()
    {
        Pressed += Launch;
    }

    public void Launch()
    {
        Dictionary info = [];
        Node3D activeThing = BuildingManager.Instance.activeThing;
        if (activeThing is Colony colony)
        {
            info.Add("sites", colony.GetPartsWithModule(typeof(LaunchSite)));
        } // ADD FUNCTIONALITY FOR CRAFTS IN THE FUTURE
        contextMenus.OpenMenu("SiteSelector", info);
    }
}
