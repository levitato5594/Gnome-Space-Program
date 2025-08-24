using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class CreditsGenerator : Panel
{
    [Export] public float showTime = 1f;
    [Export] public VBoxContainer creditContainer;
    [Export] public PackedScene personRole;

    bool alreadyVisible = false;
    public override void _Process(double delta)
    {
        if (Visible && !alreadyVisible)
        {
            alreadyVisible = true;
            GD.Print("Making credits...");
            List<string> configList = ConfigUtility.GetConfigs(ConfigUtility.GameData, "credits");
            GD.Print(configList.Count);
            foreach (string cfg in configList)
            {
                Dictionary data = ConfigUtility.ParseConfig(cfg);
                GenerateCredits(data);
            }
        }else if (!Visible && alreadyVisible){
            GD.Print("Deleting credits...");
            alreadyVisible = false;
            foreach (Node node in creditContainer.GetChildren())
            {
                node.QueueFree();
            }
        }
    }

    public void GenerateCredits(Dictionary data)
    {
        GD.Print("Making this one");
        string category = (string)data["category"];
        Label categoryLabel = new()
        {
            Text = category,
            HorizontalAlignment = HorizontalAlignment.Center
        };
        creditContainer.AddChild(categoryLabel);

        foreach (Dictionary person in ((Godot.Collections.Array)data["list"]).Select(v => (Dictionary)v))
        {
            HBoxContainer theThing = (HBoxContainer)personRole.Instantiate();
            ((Label)theThing.FindChild("Person")).Text = (string)person["name"];
            ((Label)theThing.FindChild("Role")).Text = (string)person["roles"];
            creditContainer.AddChild(theThing);
        }
    }
}
