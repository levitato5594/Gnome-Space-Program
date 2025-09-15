using Godot;
using Godot.Collections;
using System;

// Loads context menus
public partial class ContextMenus : Control
{
    [Export] public Array<Control> menus;

    public void OpenMenu(string name, Dictionary info, bool closeOtherMenus = false)
    {
        foreach (Control ctrl in menus)
        {
            if (ctrl.Name == name)
            {
                if (ctrl is ContextMenu menu)
                {
                    ctrl.Visible = true;
                    menu.Opened(info);
                }
            }else if (closeOtherMenus){
                ctrl.Visible = false;
            }
        }
    }
}
