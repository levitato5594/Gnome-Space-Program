# Base class for GDscript based part modules to get easy access to various part signals.

extends Node
class_name PartModule

# Either I haven't looked enough or what but I can't seem to get the FUCKING class name from within Part.cs
# As such, Part.cs will check if a script has this, and then assume it's a part module.
var identification:String = "PartModule"

var part_node # Assign this upon part initialization

# Update function controlled by Part.cs
# Key difference to the Process function is that this one does not run if the part is disabled.
func update():
	pass

# Invoked when the part is initialized
func part_init():
	pass
