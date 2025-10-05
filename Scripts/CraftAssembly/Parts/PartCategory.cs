using Godot;
using System;

public partial class PartCategory
{
    public string name;
    public string displayName;
    public string description;
    public string iconPath;

    public Image iconImg;

    // This will throw a warning if we're using 
    public void LoadImg()
    {
        Image image = new();

        Error err = image.Load($"{ConfigUtility.GameData}/{iconPath}");
        if (err != Error.Ok)
        {
            Logger.Print("[color=red]Failed to load image!");
            return;
        }

        iconImg = image;
    }
}
