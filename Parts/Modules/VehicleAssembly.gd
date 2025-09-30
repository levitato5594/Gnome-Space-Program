# A lot of part construction logic is handled within internal C# classes.
# This module simply invokes various methods where necessary.

extends PartModule
class_name VehicleAssembly

var craft_manager

var activeSave = SingletonRegistry.registry["ActiveSave"]

func part_init():
	GlobalLogger.GDPrint(activeSave)
	GlobalLogger.GDPrint(part_node.name)
