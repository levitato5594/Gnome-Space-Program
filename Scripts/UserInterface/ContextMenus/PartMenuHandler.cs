using Godot;
using Godot.Collections;

public partial class PartMenuHandler : Node
{
    public static PartMenuHandler Instance { get; private set; }
    [Export] public ContextMenus contextMenus;
    [Export] public PackedScene menuPrefab;
    [Export] public float rayLength = 1000;

    public Part hoveredPart;

    public override void _Ready()
    {
        Instance = this;
        SingletonRegistry.Register(this);
    }

    public void CreateMenu(Part part)
    {
        ulong partInstanceID = part.GetInstanceId();
        string partName = $"{partInstanceID}_MENU";

        Dictionary info = [];

        if (contextMenus.GetMenu(partName) == null)
        {
            PartMenu partMenu = (PartMenu)menuPrefab.Instantiate();
            partMenu.Name = partName;
            partMenu.menus = contextMenus;
            contextMenus.AddChild(partMenu);
            contextMenus.menus.Add(partMenu);
        }
        
        info.Add("part", part);
        info.Add("mousePos", GetViewport().GetMousePosition());
        info.Add("buttons", part.buttons);

        contextMenus.OpenMenu(partName, info);
    }

    // MOVE THIS TO PARTMANAGER.CS SOON
    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventMouseMotion mouseMotion)
        {
            hoveredPart = null;
            Camera3D camera3D = ActiveSave.Instance.localCamera;
            PhysicsDirectSpaceState3D spaceState = ActiveSave.Instance.localSpace.GetWorld3D().DirectSpaceState;

            Vector3 from = camera3D.ProjectRayOrigin(mouseMotion.Position);
            Vector3 to = from + camera3D.ProjectRayNormal(mouseMotion.Position) * rayLength;

            PhysicsRayQueryParameters3D rayParams = new(){From = from, To = to};
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

        if (@event is InputEventMouseButton mouseButton)
        {
            if (mouseButton.ButtonIndex == MouseButton.Right && mouseButton.Pressed)
            {
                if (hoveredPart != null)
                {
                    CreateMenu(hoveredPart);
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

        return part.IsVisibleInTree() && selectionStatus;
    }
}
