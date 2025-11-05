using Godot;
using Godot.Collections;
using System;

public partial class LaunchsiteMenu : ContextMenu
{
    [Export] public VBoxContainer buttonContainer;
    public override void Opened(Dictionary info)
    {
        Array<Node> sites = (Array<Node>)info["sites"];
        foreach (Node site in sites)
        {
            if (site is Part launchsite)
            {
                BuildModeLaunch launchButton = new();
                buttonContainer.AddChild(launchButton);
                launchButton.launchsite = launchsite;
            }
        }
    }
}
