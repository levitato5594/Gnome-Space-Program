using Godot;
using System;
using System.Collections.Generic;

public partial class PartList : Control
{
    [Export] public PackedScene buttonPrefab;
	[Export] public PackedScene categoryPrefab;
	[Export] public PackedScene partSelector;
	[Export] public VBoxContainer categoryList;
    [Export] public Control partPreviewContainer;
    [Export] public Button allButton;

    public Dictionary<string, PartCategoryContainer> categories = [];

    public void FreePartList()
    {
        Logger.Print("Clearing parts list");
        // Clear stuff
        categories.Clear();
        foreach (Node node in GetChildren())
		{
            node.QueueFree();
        }
		foreach (Node node in categoryList.GetChildren())
		{
            node.QueueFree();
        }
    }

    public void LoadPartList()
    {
        FreePartList();

        PartCategoryContainer allCategory = (PartCategoryContainer)categoryPrefab.Instantiate();
        AddChild(allCategory);
        allCategory.button = allButton;
        allCategory.Initialize();

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
            if (part.Value.listedInSelector)
            {
               string categoryName = part.Value.category;

                // Load all parts into the "all" category
                PartSelector allSelector = (PartSelector)partSelector.Instantiate();
                allSelector.Name = $"{part.Key}_AllSelector";
                allSelector.partRef = part.Value;
                allSelector.partPreviewContainer = partPreviewContainer;

                allCategory.container.AddChild(allSelector);
                allSelector.LoadPart();

                if (categories.TryGetValue(categoryName, out PartCategoryContainer categoryContainer))
                {
                    PartSelector selector = (PartSelector)partSelector.Instantiate();
                    selector.Name = $"{part.Key}_Selector";
                    selector.partRef = part.Value;
                    selector.partPreviewContainer = partPreviewContainer;

                    categoryContainer.container.AddChild(selector);
                    selector.LoadPart();
                } 
            }
        }

        // Open
        allCategory.Visible = false;
        allCategory.OnClick();
    }
}
