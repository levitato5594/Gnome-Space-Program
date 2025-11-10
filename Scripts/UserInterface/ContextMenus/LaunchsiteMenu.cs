using Godot;
using Godot.Collections;

public partial class LaunchsiteMenu : ContextMenu
{
    [Export] public VBoxContainer buttonContainer;
    public override void Opened(Dictionary info)
    {
        // Vaporize old buttons
        foreach (Node node in buttonContainer.GetChildren())
        {
            node.QueueFree();
        }
        Array<Node> sites = (Array<Node>)info["sites"];
        foreach (Node site in sites)
        {
            if (site is Part launchsite)
            {
                LaunchSite launchsiteModule = (LaunchSite)launchsite.GetPartModules(typeof(LaunchSite))[0];

                BuildModeLaunch launchButton = new();
                buttonContainer.AddChild(launchButton);
                launchButton.launchsite = launchsite;
                launchButton.Text = launchsiteModule.siteName;
            }
        }
    }
}
