using Godot;
using System;
using System.Collections.Generic;

public partial class PartList : Control
{
    [Export] public PackedScene buttonPrefab;
	[Export] public PackedScene categoryPrefab;
	[Export] public PackedScene partSelector;
	[Export] public VBoxContainer categoryList;

    public Dictionary<string, PartCategoryContainer> categories = [];

    public void LoadPartList()
    {
        // Clear existing stuff
		categories.Clear();
        foreach (Node node in GetChildren())
		{
            node.QueueFree();
        }
		foreach (Node node in categoryList.GetChildren())
		{
            node.QueueFree();
        }

        foreach (KeyValuePair<string, PartCategory> category in PartManager.Instance.partCategories)
		{
            PartCategoryContainer categoryObject = (PartCategoryContainer)categoryPrefab.Instantiate();

            Button button = (Button)buttonPrefab.Instantiate();
            categoryList.AddChild(button);
			AddChild(categoryObject);

            TextureRect buttonTex = (TextureRect)button.FindChild("THETEXTURE");
            ImageTexture tex = new();
            tex.SetImage(category.Value.iconImg);
            buttonTex.Texture = tex;

            categoryObject.button = button;
            categoryObject.Initialize();

            categories.Add(category.Key, categoryObject);
        }

        foreach (KeyValuePair<string, CachedPart> part in PartManager.Instance.partCache)
		{
            string categoryName = part.Value.category;
            if (categories.TryGetValue(categoryName, out PartCategoryContainer categoryContainer))
			{
				PartSelector selector = (PartSelector)partSelector.Instantiate();
                selector.Name = $"{part.Key}_Selector";
                selector.partRef = part.Value;
                categoryContainer.AddChild(selector);
                selector.LoadPart();
            }
        }
    }
}
