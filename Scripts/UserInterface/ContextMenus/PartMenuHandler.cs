using Godot;
using Godot.Collections;

public partial class PartMenuHandler : Node
{
    public static PartMenuHandler Instance { get; private set; }
    [Export] public ContextMenus contextMenus;
    [Export] public PackedScene menuPrefab;

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

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseButton)
        {
            if (mouseButton.ButtonIndex == MouseButton.Right && mouseButton.Pressed)
            {
                if (PartManager.Instance.hoveredPart != null)
                {
                    CreateMenu(PartManager.Instance.hoveredPart);
                }
            }
        }
    }
}
