using Godot;
using Godot.Collections;

public partial class BuildModeLaunch : Button
{
    public LaunchSite launchsite;
    public override void _Ready()
    {
        Pressed += Launch;
    }

    public void Launch()
    {
        // Get the current list of parts in the editor (you can move this to the LaunchSite module if you want)
        Dictionary partData = PartManager.CompilePartData(BuildingManager.Instance.partsList);
        launchsite.SpawnCraft(partData, true);
    }
}
