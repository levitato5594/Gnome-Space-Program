using Godot;
using System;

// Class for buttons to switch, close, or open UI
public partial class UISwitcher : Button
{
    [Export] public bool toggler;
    [Export] public Control toClose; // Leave null to not close current UI
    [Export] public Control toOpen; // Leave null to not open a new UI

    public override void _Pressed()
    {
        if (toClose != null) toClose.Visible = false;

        if (toOpen != null) 
        {
            if (!toggler)
            {
                toOpen.Visible = true;
            }
            else
            {
                toOpen.Visible = !toOpen.Visible;
            }
        }
    }
}
