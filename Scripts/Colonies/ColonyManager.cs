using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

// Helper singleton for managing colonies
public partial class ColonyManager : Node
{
    public static readonly string classTag = "([color=LEMON_CHIFFON]ColonyManager[color=white])";
    public static ColonyManager Instance { get; private set; }
    [Export] public PackedScene iconPrefab;
    [Export] public Control iconParent;

    public override void _Ready()
    {
        Instance = this;
        GD.PrintRich($"{classTag} ColonyManager Ready!");
    }

    public List<Colony> ParseColonies(string path, bool blueprint = false)
    {
        GD.PrintRich($"{classTag} Parsing path: {path}");
        List<Colony> colonies = [];
        List<string> configs = ConfigUtility.GetConfigs($"{ConfigUtility.GameData}/{path}", "Base");
        GD.PrintRich($"{classTag} {configs.Count}");
        foreach (string cfg in configs)
        {
            Colony colony = ParseColony(cfg, blueprint);
            if (colony != null)
            {
                colonies.Add(colony);
            }
        }

        return colonies;
    }
    public Colony ParseColony(string configPath, bool blueprint = false)
    {
        GD.PrintRich($"{classTag} Parsing colony: {configPath}");
        Dictionary data = ConfigUtility.ParseConfig(configPath);

        // This isn't what I wanted get it outta here
        if ((bool)data["blueprint"] != blueprint)
        {
            return null;
        }

        Colony colony = new()
        {
            name = (string)data["name"]
        };

        // Grrrrr...
        if (ConfigUtility.TryGetArray("position", data, out Godot.Collections.Array posArr))
        {
            colony.position = new Double3((double)posArr[0], (double)posArr[1], (double)posArr[2]);
        }
        
        if (ConfigUtility.TryGetArray("rotation", data, out Godot.Collections.Array rotArr))
        {
            colony.rotation = new Double3((double)rotArr[0], (double)rotArr[1], (double)rotArr[2]);
        }

        // Yeah.....
        colony.Position = colony.position.ToFloat3();

        // Add to the planet and also add a map icon
        CelestialBody parent = PlanetSystem.Instance.FindCBodyByName((string)data["parent"]);
        parent.AddChild(colony);

        ScaledObject scaledObject = new() {Name = colony.name};
        PlanetSystem.Instance.scaledSpace.AddChild(scaledObject);
        scaledObject.counterpart = colony;
        scaledObject.originalScale = Double3.One * 100000000; // For max zoom reasons

        // HHhhhhmmmmmmmmmm....
        MapIcon icon = (MapIcon)iconPrefab.Instantiate();
        iconParent.AddChild(icon);
        icon.thing = scaledObject;
        icon.camera = ActiveSave.Instance.localCamera;

        //if (ConfigUtility.TryGetDictionary("buildings", data, out Dictionary buildings))
        //{
        //    colony.savedParts = buildings;
        //}

        return null;
    }
}
