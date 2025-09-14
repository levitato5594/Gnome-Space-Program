using Godot;
using Godot.Collections;
using System.Collections.Generic;

/* 
Definitions for an unloaded part in a specific scenario (in a craft, or in a colony)
instantiation is handled by the class that owns an instance of this (Craft / Colony)
*/
public partial class UnloadedPart
{
    public CachedPart template;
    public int attachNode; // -1 means radially attached.
    public Vector3 position; // relative to attachment node if attachNode >= 0
    public Vector3 rotation;
    public Dictionary additionalData; // Any additional information
    public List<UnloadedPart> attachedParts;
}
