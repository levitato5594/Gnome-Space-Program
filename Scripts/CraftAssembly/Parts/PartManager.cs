using Godot;
using Godot.Collections;
using System;

public partial class PartManager : Node
{
    public static readonly string classTag = "([color=pink]PartManager[color=white])";
    public static PartManager Instance { get; private set; }
    public System.Collections.Generic.Dictionary<string, CachedPart> partCache;

    public override void _Ready()
    {
        Instance = this;
        GD.PrintRich($"{classTag} PartManager ready!");
    }

    // Ahh the beautiful monolithic function! 
    // The truest form of programming, not giving a shit and living with your mistakes.
    public static Part ParsePartConfig(string path)
    {
        Dictionary config = ConfigUtility.ParseConfig(path);
        
        return new Part(); // Return an empty part for now because I DIDN'T FINISH THIS FUNCTION YET
    }

    public static Part InstantiatePart(CachedPart cachedPart)
    {
        Part part = new();

        
        return new Part();
    }
}