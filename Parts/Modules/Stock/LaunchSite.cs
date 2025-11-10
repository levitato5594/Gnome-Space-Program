using Godot;
using System;

/*
    This part module is BUILT IN to GSP.
    As such, this module should remain internal and not be compiled as a mod.
*/
public partial class LaunchSite : PartModule
{
    public string siteName;

    public override void PartInit() 
    {
        siteName = (string)configData["siteName"];
    }
}
