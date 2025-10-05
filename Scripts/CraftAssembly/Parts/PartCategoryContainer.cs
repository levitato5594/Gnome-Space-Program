using Godot;
using System;

public partial class PartCategoryContainer : ScrollContainer
{
	[Export] public GridContainer container;
	public Button button;
	// Called when the node enters the scene tree for the first time.
	public void Initialize()
	{
        button.Pressed += OnClick;
    }

	public void OnClick()
	{
		Control categoryParent = (Control)GetParent();
		foreach (Node child in categoryParent.GetChildren())
		{
			if (child is Control control)
			{
				control.Visible = false;
			}
		}
		Visible = true;
	}
}