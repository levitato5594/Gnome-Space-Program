using Godot;
using System;

// Button that reports its own name
public partial class SignalButton : Button
{
    public string id;
    public delegate void SendPressed(string name);
    public event SendPressed SendPress;

    public override void _Ready()
    {
        Pressed += OnPress;
    }

    public void OnPress()
    {
        SendPress?.Invoke(id);
    }
}
