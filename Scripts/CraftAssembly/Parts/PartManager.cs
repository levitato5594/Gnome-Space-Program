using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public partial class PartManager : Node
{
    [Export] public PartMenuHandler partMenus;
    [Export] public float rayLength = 1000;
    public static readonly string classTag = "([color=pink]PartManager[color=white])";
    public static PartManager Instance { get; private set; }

    public System.Collections.Generic.Dictionary<string, CachedPart> partCache = [];
    public System.Collections.Generic.Dictionary<string, PartCategory> partCategories = [];

    public Part hoveredPart;

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
                scenePath = (string)data["partScene"],
                listedInSelector = (bool)data["listedInSelector"]
            };

            parts.Add(part);
        }

        return parts;
    }

    // We shoot a ray occasionally to get the current hovered part. Can be used for all sorts of part selection shenanigans.
    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventMouseMotion mouseMotion)
        {
            hoveredPart = null;
            Camera3D camera3D = ActiveSave.Instance.localCamera;
            PhysicsDirectSpaceState3D spaceState = ActiveSave.Instance.localSpace.GetWorld3D().DirectSpaceState;

            Vector3 from = camera3D.ProjectRayOrigin(mouseMotion.Position);
            Vector3 to = from + camera3D.ProjectRayNormal(mouseMotion.Position) * rayLength;

            PhysicsRayQueryParameters3D rayParams = new() { From = from, To = to };
            Dictionary result = spaceState.IntersectRay(rayParams);

            //Logger.Print(result);
            if (result.Count > 0)
            {
                Node colliderResult = (Node)result["collider"];

                if (colliderResult is Part part && part.IsVisibleInTree())
                {
                    // Add logic for craft later too
                    if (IsPartSelectable(part))
                    {
                        hoveredPart = part;
                    }
                }
            }
        }
    }

    public bool IsPartSelectable(Part part)
    {
        int editorMode = BuildingManager.Instance.editorMode;

        bool selectionStatus = false;
        // Add case for dynamic editing when EVA construction becomes relevant
        switch (editorMode)
        {
            case (int)BuildingManager.EditorMode.Static: // If we're editing a static thing
                if (part.inEditor) selectionStatus = true;
                break;
            default: // Selection logic for if we're not editing
                if (ActiveSave.Instance.activeThing is Colony colony && part.parentThing == colony) selectionStatus = true;
                if (ActiveSave.Instance.activeThing is Craft && part.parentThing is not Colony) selectionStatus = true;
                break;
        }

        return part.IsVisibleInTree() && selectionStatus && BuildingManager.Instance.draggingPart != part;
    }
}