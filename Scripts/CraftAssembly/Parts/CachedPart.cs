using Godot;
using System;

public partial class CachedPart
{
    public string name;
    public string displayName;
    public string category;
    public string pckFile;
    public string scenePath;

    // Dynamic stuff - self assigned
    public PackedScene partScene;

    // Run this ONLY ONCE per part!
    public void LoadPartFiles()
    {
        // Dangerous operation. This will actively "install" resources into the game. 
        // Follow the part modding convention! 
        bool success = ProjectSettings.LoadResourcePack(pckFile);

        if (!success)
        {
            GD.Print($"({name}) Failed to load resource pack '{pckFile}'.");
            GD.Print($"({name}) Attempting to forcefully load scene '{scenePath}'...");
        }

        // Errors out if the scene doesn't exist. The only reason this even tries to load anyways is for testing.
        PackedScene scene = (PackedScene)ResourceLoader.Load($"{ConfigUtility.GameData}/{scenePath}");
        partScene = scene;
        GD.Print($"({name}) Scene loading success!");
    }
}
