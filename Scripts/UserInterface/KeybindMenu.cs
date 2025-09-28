using Godot;
using System;

public partial class KeybindMenu : Control
{
    [Export] public Godot.Collections.Array<Key> keys;
    int keysPressed = 0;

    // Pretty gross solution but it works a little
    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventKey keyEvent)
        {
            if (keys.Contains(keyEvent.Keycode))
            {
                if (keyEvent.Pressed)
                {
                    keysPressed = keysPressed < keys.Count ? keysPressed + 1 : 0;
                }else{
                    keysPressed = keysPressed > 0 ? keysPressed - 1 : 0;
                }
            }
        }

        if (keysPressed >= keys.Count)
        {
            Logger.Print($"{Name} Visibility is {!Visible}");
            Visible = !Visible;
        }
    }
}
