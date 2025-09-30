using Godot;
using Godot.Collections;
using System;

public partial class PartMenu : ContextMenu
{
    [Export] public Label title;
    [Export] public DraggablePanel dragMenu;
    public ContextMenus menus;
    public override void Opened(Dictionary info) 
    {
        Part part = (Part)info["part"];
        title.Text = part.cachedPart.displayName;

        Vector2 mousePos = (Vector2)info["mousePos"];
        dragMenu.Position = mousePos + new Vector2(-10,-10);
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseButton)
        {
            if (mouseButton.Pressed && mouseButton.ButtonIndex == MouseButton.Left)
            {
                menus.menus.Remove(this);
                QueueFree();
            }
        }
    }
}
