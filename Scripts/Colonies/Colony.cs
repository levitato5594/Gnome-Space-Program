using Godot;
using Godot.Collections;
using System.Collections.Generic;
using System.Linq;

public partial class Colony : Node
{
    /*
    Colonies also use "parts" to take advantage of the attachment node system.
    In return, craft get to take advantage of stuff like launchsites and VABs.

    TODO: Instantiate the class within a planet and have it show up in the map
    */

    public string name;
    public Double3 position;
    public Double3 rotation;
    public Dictionary cachedParts;

    public Part rootPart;
    public List<Part> allParts;

    public void Load()
    {
        foreach (var pair in cachedParts)
        {
            string name = (string)pair.Key;
            Dictionary data = (Dictionary)pair.Value;

            // ... Now we need to make a part manager.....
        }
    }

    public void Unload()
    {
        
    }
}