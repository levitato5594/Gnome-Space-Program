using Godot;
using System;

public partial class DropdownButton : BaseButton
{
    [Export] public Container container;

    public override void _Toggled(bool toggledOn)
    {
        if (container != null) container.Visible = !toggledOn;
    }
}