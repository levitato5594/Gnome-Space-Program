using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public partial class PartMenu : ContextMenu
{
    [Export] public Label title;
    [Export] public VBoxContainer itemList;
    [Export] public DraggablePanel dragMenu;
    public ContextMenus menus;
    public Part part;

    public override void Opened(Dictionary info) 
    {
        title.Text = part.cachedPart.displayName;

        Vector2 mousePos = (Vector2)info["mousePos"];
        dragMenu.Position = mousePos + new Vector2(-10,-10);

        //foreach (Node child in itemList.GetChildren()) child.QueueFree();
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseButton)
        {
            if (mouseButton.Pressed && mouseButton.ButtonIndex == MouseButton.Left)
            {
                Visible = false;
            }
        }
    }
}
