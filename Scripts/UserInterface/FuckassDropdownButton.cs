using Godot;
using System;

// This fucking mistake is born from the fact that TextureButton doesn't inherit from Button
public partial class FuckassDropdownButton : Button
{
    [Export] public Container container;

    public override void _Toggled(bool toggledOn)
    {
        if (container != null) container.Visible = toggledOn;
    }
}
