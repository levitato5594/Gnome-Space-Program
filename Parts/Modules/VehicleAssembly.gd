# A lot of part construction logic is handled within internal C# classes.
# This module simply sets everything up

extends PartModule
class_name VehicleAssembly

@export var maxZoom:int = 35
@export var targetZoom:int = 1
@export var camPivot:Node3D

@export var pivotTgtPos:Vector3
@export var pivotMove:float = 1.25
@export var pivotMoveSpeed:float = 10
@export var pivotMinHeight:float = 0
@export var pivotMaxHeight:float = 80

var enterButtonName:String = "Enter"

var craft_manager

var activeSave = SingletonRegistry.registry["ActiveSave"]
var flightCam = SingletonRegistry.registry["FlightCamera"]
var partMenuHandler = SingletonRegistry.registry["PartMenuHandler"]
var buildingManager = SingletonRegistry.registry["BuildingManager"]

var verticalScroll = false;
var active = false;

func _process(delta: float):
	camPivot.position = camPivot.position.lerp(pivotTgtPos, pivotMoveSpeed * delta)

func part_init():
	part_node.AddButton(enterButtonName, "Building Controls")

	# Connect button events
	part_node.SendButton.connect(button_handler)

func button_handler(buttonID:String):
	if buttonID == enterButtonName:
		# Reposition cam and disable selection for the colony
		flightCam.TargetObject(camPivot, Vector3(0.1,maxZoom,targetZoom), false)
		buildingManager.editorMode = 1 # "Static"

		# Disable map view while in flight
		flightCam.ToggleMapView(false)
		flightCam.canEnterMap = false

		# Close Part Menus
		partMenuHandler.contextMenus.OpenMenu("", [], true)

		active = true;

# We sort of hijack the flight camera for this.. It should be fine though.... :glueless:
func _unhandled_input(event: InputEvent):
	if event is InputEventKey:
		if active:
			if event.keycode == Key.KEY_SHIFT && event.pressed:
				verticalScroll = true;
				flightCam.canZoom = false;
			else:
				verticalScroll = false;
				flightCam.canZoom = true;
		else:
			verticalScroll = false;

	if event is InputEventMouseButton:
		if event.button_index == MouseButton.MOUSE_BUTTON_WHEEL_UP && verticalScroll:
			if pivotTgtPos.y < pivotMaxHeight: pivotTgtPos.y += pivotMove
		if event.button_index == MouseButton.MOUSE_BUTTON_WHEEL_DOWN && verticalScroll:
			if pivotTgtPos.y > pivotMinHeight: pivotTgtPos.y -= pivotMove