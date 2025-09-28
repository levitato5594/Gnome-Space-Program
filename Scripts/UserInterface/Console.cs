using Godot;
using System;

public partial class Console : RichTextLabel
{
    bool ctrlHeld = false;
    public override void _EnterTree()
    {
        GuiInput += OnInput;
        Logger.OnLogged += AddLog;
        Text = "";
        Logger.Print("Ingame console ready");
        Logger.Print(" _______    _______   _______");
        Logger.Print("/          /         /       \\");
        Logger.Print("|   ____   \\______   |_______/");
        Logger.Print("|       \\         \\  |");
        Logger.Print("\\_______/  _______/  |");
        Logger.Print("");
    }

    public void AddLog(DateTime time, string content)
    {
        Text += $"[color=676767]{time:HH:mm:ss}[color=white]: {content}\n";
    }

    public override void _UnhandledInput(InputEvent inpEvent)
    {
        if (inpEvent is InputEventKey key)
        {
            if (key.Pressed && key.Keycode == Key.Ctrl)
            {
                ctrlHeld = true;
            }else if (!key.Pressed && key.Keycode == Key.Ctrl)
            {
                ctrlHeld = false;
            }
        }
    }

    public void OnInput(InputEvent inpEvent)
    {
        if (inpEvent is InputEventMouseButton mouseMotion && ctrlHeld)
        {
            if (mouseMotion.ButtonIndex == MouseButton.WheelUp)
            {
                AddThemeFontSizeOverride("normal_font_size", GetThemeFontSize("normal_font_size") + 1);
            }
            if (mouseMotion.ButtonIndex == MouseButton.WheelDown)
            {
                AddThemeFontSizeOverride("normal_font_size", GetThemeFontSize("normal_font_size") - 1);
            }
        }
    }
}
