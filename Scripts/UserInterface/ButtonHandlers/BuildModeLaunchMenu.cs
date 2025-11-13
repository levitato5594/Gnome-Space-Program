using Godot;
using Godot.Collections;
using System;

public partial class BuildModeLaunchMenu : Button
{
    [Export] public ContextMenus contextMenus;
    [Export] public string menuName = "SiteSelector";
    public override void _Ready()
    {
        Pressed += Launch;
    }

    public void Launch()
    {
        ContextMenu menu = contextMenus.GetMenu(menuName);
        if (menu.Visible)
        {
            menu.Visible = false;
        }else{
            Dictionary info = [];
            Node3D activeThing = BuildingManager.Instance.activeThing;
            if (activeThing is Colony colony)
            {
                info.Add("sites", colony.GetPartsWithModule(typeof(LaunchSite)));
            } // ADD FUNCTIONALITY FOR CRAFTS IN THE FUTURE
            contextMenus.OpenMenu(menuName, info);
        }
    }
}
