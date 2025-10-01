# A lot of part construction logic is handled within internal C# classes.
# This module simply sets everything up

extends PartModule
class_name VehicleAssembly

var enterButtonName:String = "Enter"

var craft_manager

var activeSave = SingletonRegistry.registry["ActiveSave"]
var flightCam = SingletonRegistry.registry["FlightCamera"]
var partMenuHandler = SingletonRegistry.registry["PartMenuHandler"]
var buildingManager = SingletonRegistry.registry["BuildingManager"]

func part_init():
	part_node.AddButton(enterButtonName, "Building Controls")

	# Connect button events
	part_node.SendButton.connect(button_handler)

func button_handler(buttonID:String):
	## 
	if buttonID == enterButtonName:
		# Reposition cam and disable selection for the colony
		flightCam.TargetObject(self, Vector3(0.1,420,5), false)
		buildingManager.editorMode = "Craft"

		# Disable map view while in flight
		flightCam.ToggleMapView(false)
		flightCam.canEnterMap = false

		# Close Part Menus
		partMenuHandler.contextMenus.OpenMenu("", [], true)
