using Godot;
using System;

public partial class CachedPart
{
    public string name;
    public string displayName;
    public string category;
    public string pckFile;
    public string scenePath;
    public bool listedInSelector = true;

    // Dynamic stuff - self assigned
    public PackedScene partScene;

    // Run this ONLY ONCE per part!
    public void LoadAssets()
    {
        // Dangerous operation. This will actively "install" resources into the game. 
        // Follow the part modding convention! 
        bool success = ProjectSettings.LoadResourcePack($"{ConfigUtility.GameData}/{pckFile}");

        if (!success)
        {
            Logger.Print($"(Cached {name}) Failed to load resource pack '{ConfigUtility.GameData}/{pckFile}'.");
            Logger.Print($"(Cached {name}) Attempting to forcefully load scene '{scenePath}'...");
        }

        // Errors out if the scene doesn't exist. The only reason this even tries to load anyways is for testing.
        PackedScene scene = (PackedScene)ResourceLoader.Load(scenePath);
        if (scene != null)
        {
            partScene = scene;
            Logger.Print($"(Cached {name}) Scene loading success!");
        }else{
            Logger.Print($"(Cached {name}) Could not load part.");
        }
    }

    public Part Instantiate(Node parent)
    {
        Logger.Print($"(Cached {name}) Instantiating...");
        Part part = (Part)partScene.Instantiate();
        parent.AddChild(part);

        return part;
    }
}
