using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

// Helper class for managing colonies
public partial class ColonyManager : Node
{
    public static List<Colony> ParseColonies(string path, bool blueprint = false)
    {
        List<Colony> colonies = [];
        List<string> configs = ConfigUtility.GetConfigs(path, "Base");
        foreach (string cfg in configs)
        {
            Colony colony = ParseColony(cfg);
            if (colony != null)
            {
                colonies.Add(colony);
            }
        }

        return colonies;
    }
    public static Colony ParseColony(string configPath, bool blueprint = false)
    {
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

        if (ConfigUtility.TryGetDictionary("buildings", data, out Dictionary buildings))
        {
            colony.cachedParts = buildings;
        }

        return null;
    }
}
