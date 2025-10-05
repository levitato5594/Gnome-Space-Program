using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public partial class PartManager : Node
{
    [Export] public PartMenuHandler partMenus;
    public static readonly string classTag = "([color=pink]PartManager[color=white])";
    public static PartManager Instance { get; private set; }

    public System.Collections.Generic.Dictionary<string, CachedPart> partCache = [];
    public System.Collections.Generic.Dictionary<string, PartCategory> partCategories = [];

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

            Logger.Print($"{classTag} Loading part categories");

            List<PartCategory> partCats = GetPartCategoriesInPack(pack);
            foreach (PartCategory cat in partCats)
            {
                partCategories.Add(cat.name, cat);
                cat.LoadImg();
            }

            Logger.Print($"{classTag} {pack.displayName} Loaded!");
            Logger.Print($"{classTag} Total parts: ({parts.Count}) Total categories: ({partCats.Count})");
        }
    }

    public static List<PartCategory> GetPartCategoriesInPack(PartPack pack)
    {
        List<PartCategory> categories = [];

        string packPath = $"{ConfigUtility.GameData}/{pack.path}";
        List<string> configs = ConfigUtility.GetConfigs(packPath, "PartCat");

        foreach (string cfgPath in configs)
        {
            Dictionary data = ConfigUtility.ParseConfig(cfgPath);

            PartCategory category = new()
            {
                name = (string)data["name"],
                iconPath = (string)data["icon"]
            };

            if (ConfigUtility.TryGetDictionary("displayData", data, out Dictionary dispData))
            {
                category.displayName = (string)dispData["displayName"];
                category.description = (string)dispData["description"];
            }

            categories.Add(category);
        }

        return categories;
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