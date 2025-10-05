using Godot;
using System;

public partial class PartSelector : Button
{
    [Export] public SubViewportContainer vpContainer;
    [Export] public Camera3D vpCam;
    [Export] public Node3D partContainer;
    [Export] public float sizeUp = 1.2f;
    [Export] public float sizeUpDuration = 0.2f;

    public CachedPart partRef;

    public override void _Ready()
    {
        MouseEntered += () => ExpandPart(true);
        MouseExited += () => ExpandPart(false);

        Pressed += OnPress;
    }

    // Loads a passive version of a part into itself and assigns all the important values
    public void LoadPart()
    {
        Part partObj = partRef.Instantiate(partContainer);
        partObj.Freeze = true;
        partObj.enabled = false;

        partObj.GlobalPosition = Vector3.Zero;

        vpCam.LookAt(partObj.CenterOfMass + partObj.GlobalPosition);
    }

    public void ExpandPart(bool expand)
    {
        Vector2 targetSize = Size;
        Vector2 targetPos = Vector2.Zero;
        vpContainer.ZIndex = 0;
        if (expand)
        {
            targetSize = Size * sizeUp;

            targetPos = -Vector2.One * ((targetSize.X - Size.X) / 2.0f);

            vpContainer.ZIndex = 100;
        }

        Tween tween = CreateTween();
        tween.SetTrans(Tween.TransitionType.Expo);
        tween.SetEase(Tween.EaseType.Out);
        tween.SetParallel(true);

        tween.TweenProperty(vpContainer, "size", targetSize, sizeUpDuration);
        tween.TweenProperty(vpContainer, "position", targetPos, sizeUpDuration);
    }

    // Instantiate a "fake" part
    public void OnPress()
    {

    }
}
