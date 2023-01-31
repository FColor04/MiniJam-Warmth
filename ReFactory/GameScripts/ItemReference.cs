using System;
using Microsoft.Xna.Framework.Graphics;

namespace ReFactory.GameScripts;

public class ItemReference
{
    public string name;
    public string tooltip;
    public int maxStack;
    public Texture2D sprite;

    public ItemReference(string name, string tooltip, Texture2D sprite, int maxStack = 64)
    {
        this.name = name;
        this.tooltip = tooltip;
        this.sprite = sprite;
        this.maxStack = maxStack;
    }

    public static void RegisterItem(string name, string tooltip, Texture2D sprite, int maxStack = 64)
    {
        ItemList.Items.Add(new ItemReference(name, tooltip, sprite, maxStack));
    }
}