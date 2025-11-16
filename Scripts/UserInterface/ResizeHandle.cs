using Godot;
using System;

public partial class ResizeHandle : Button
{
    [Export] public Control target;
    [Export] public Vector2 direction = new(0,1);
    bool mouseInWindow = false;
    bool dragging = false;
    Vector2 offset;
    Vector2 sizeOffset;
    Vector2 posOffset;

    public override void _Ready()
    {
        // Just set everything up here. No need for the editor.
        GuiInput += GuiEvent;
    }

    public void GuiEvent(InputEvent inpEvent)
    {
        if (inpEvent is InputEventMouseButton buttonEvent)
        {
            if (buttonEvent.ButtonIndex == MouseButton.Left)
            {
                if (buttonEvent.Pressed)
                {
                    dragging = true;
                    offset = GetGlobalMousePosition();
                    sizeOffset = target.Size;
                    posOffset = target.Position;
                }else{
                    dragging = false;
                }
            }
        }else if (inpEvent is InputEventMouseMotion && dragging){
            target.Size = direction * (GetGlobalMousePosition() - offset) + sizeOffset;

            if (direction.Y < 0 && target.Size.Y > target.CustomMinimumSize.Y)
                target.Position = new Vector2(target.Position.X, (direction.Abs() * (GetGlobalMousePosition() - offset) + posOffset).Y);

            if (direction.X < 0 && target.Size.X > target.CustomMinimumSize.X)
                target.Position = new Vector2((direction.Abs() * (GetGlobalMousePosition() - offset) + posOffset).X, target.Position.Y);
        }
    }
}
