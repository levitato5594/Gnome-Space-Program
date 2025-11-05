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
        //info.Add("sites", BuildingManager.Instance.activeColony.GetPartsWithModule("launchsite"));
        contextMenus.OpenMenu("SiteSelector", info);
    }
}
