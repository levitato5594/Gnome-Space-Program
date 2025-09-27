using Godot;
using System;

public partial class Console : RichTextLabel
{
    public override void _EnterTree()
    {
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
}
