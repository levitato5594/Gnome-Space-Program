using Godot;
using Godot.Collections;
using System;

// Parts saved in things like unloaded crafts and shiiiiii
public partial class SavedPart : Node
{
    public CachedPart reference;

    public int id;

    // Just anything really
    public Dictionary partData;

    public void GenerateID()
    {
        int randInt = (int)Math.Abs(GD.Randi());
        Logger.Print($"{reference.displayName} generated new id {randInt}");

        id = randInt;
    }
}
