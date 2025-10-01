using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public partial class PartMenu : ContextMenu
{
    [Export] public Label title;
    [Export] public VBoxContainer itemList;
    [Export] public DraggablePanel dragMenu;
    [Export] public PackedScene buttonPrefab;
    public ContextMenus menus;

    public Godot.Collections.Dictionary<string, Button> buttonDict = [];

    public override void Opened(Dictionary info) 
    {
        Part part = (Part)info["part"];
        title.Text = part.cachedPart.displayName;

        Vector2 mousePos = (Vector2)info["mousePos"];
        dragMenu.Position = mousePos + new Vector2(-10,-10);

        foreach (Node child in itemList.GetChildren()) child.QueueFree();
        buttonDict.Clear();
        MakeButtons(part, (Dictionary)info["buttons"]);
    }

    public void MakeButtons(Part part, Dictionary buttons)
    {
        foreach (KeyValuePair<Variant, Variant> butt in buttons)
        {
            SignalButton button = (SignalButton)buttonPrefab.Instantiate();

            itemList.AddChild(button);
            button.AddThemeFontSizeOverride("font_size", 14);

            button.Text = (string)butt.Key;
            button.id += (string)butt.Key;

            buttonDict.Add((string)butt.Key, button);

            button.SendPress += part.SendButtonEvent;
        }
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
