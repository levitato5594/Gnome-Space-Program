using Godot;
using Godot.Collections;
using System;

// Every part module should inherit from this
public partial class PartModule
{
    // Called when a part is "started" i guess
    public virtual void PartInit(Dictionary configData) {}
}
