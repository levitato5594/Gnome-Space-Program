using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public partial class PartManager : Node
{
    public static readonly string classTag = "([color=pink]PartManager[color=white])";
    public static PartManager Instance { get; private set; }

    public System.Collections.Generic.Dictionary<string, CachedPart> partCache = [];

    public override void _Ready()
    {
        Instance = this;
        Logger.Print($"{classTag} PartManager ready!");
    }

    public void LoadPartPacks(List<PartPack> packs)
    {
        foreach (PartPack pack in packs)
        {
            Logger.Print($"{classTag} Loading part pack '{pack.displayName}'...");
            List<CachedPart> parts = GetPartsInPack(pack);
            foreach (CachedPart part in parts)
            {
                partCache.Add(part.name, part);
                part.LoadAssets();
            }
        }
    }

    public static List<CachedPart> GetPartsInPack(PartPack pack)
    {
        List<CachedPart> parts = [];

        string packPath = $"{ConfigUtility.GameData}/{pack.path}";
        List<string> configs = ConfigUtility.GetConfigs(packPath, "PartDef");

        foreach (string cfgPath in configs)
        {
            Dictionary data = ConfigUtility.ParseConfig(cfgPath);

            CachedPart part = new()
            {
                name = (string)data["name"],
                displayName = (string)data["displayName"],
                category = (string)data["category"],
                pckFile = (string)data["assets"],
                scenePath = (string)data["partScene"]
            };

            parts.Add(part);
        }

        return parts;
    }
}