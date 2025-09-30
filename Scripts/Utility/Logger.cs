using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Logger : Node
{
    public static Logger Instance { get; private set; }

    public Dictionary<DateTime, string> LoggedMessages = [];

    // Events 
    public delegate void CatchLog(DateTime time, string content);
    public static event CatchLog OnLogged;

    // The first thing!!
    public override void _EnterTree()
    {
        Instance = this;

        Print("[color=45ffdb]Logger active.");
    }

    public static void Print(object content)
    {
        string text = content.ToString();
        DateTime time = DateTime.Now;

        Instance.LoggedMessages.Add(time, text);
        GD.PrintRich($"[color=676767]{time:HH:mm:ss}[color=white]: {text}");
        OnLogged?.Invoke(time, text);
    }

    // Printing from GDScript
    public static void GDPrint(Variant content)
    {
        Print(content);
    }
}
