using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class MainMenuMultiOption : Control
{
    public SaveParam param;
    [Export] public Label title;
    [Export] public VBoxContainer itemCont;
    public override void _Ready()
    {
        Godot.Collections.Array<Node> items = itemCont.GetChildren();
        ((CheckButton)items[0]).ButtonPressed = true;
        //popup.SetItemChecked(0, true);
    }
    public override void _Process(double delta)
    {
        List<Variant> options = [];
        Godot.Collections.Array<Node> items = itemCont.GetChildren();

        foreach (Node item in items)
        {
            CheckButton check = (CheckButton)item;
            if (check.ButtonPressed)
            {
                options.Add(check.Text); // ughghhh this is SO ASS but it'll be the name for now because fuck you
            }
        }

        Godot.Collections.Array<Variant> fuckingArray = [];
        foreach (Variant item in options)
        {
            fuckingArray.Add(item);
        }

        param.inputData.currentSelection = fuckingArray;
    }
}
