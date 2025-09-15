using Godot;

public partial class ScaledObject : Node3D
{
    public Node3D counterpart;
    public Double3 truePosition = Double3.Zero;
    public Double3 originalScale = Double3.One;

    public override void _Process(double delta)
    {
        ForceUpdate();
    }

    public void ForceUpdate()
    {
        // If the counterpart is null then assume that another object is handling positioning
        if (counterpart != null) truePosition = Double3.ConvertToDouble3(counterpart.GlobalPosition);
    }
}
