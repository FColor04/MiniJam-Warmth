using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MiniJam_Warmth.GameScripts;

namespace MiniJam_Warmth;

public class ItemSlot : UIElement, IDragHandler, IDropHandler
{
    public ItemSlot(UI.Margin margin, Texture2D texture = null, Color? color = null,
        List<UIElement> children = null, int priority = -1, float rotation = 0) : this(margin.GetRect, texture, color, children, priority, rotation) { }

    public ItemSlot(Rectangle? rect = default, Texture2D texture = null, Color? color = null,
        List<UIElement> children = null, int priority = -1, float rotation = 0) : base(rect, texture, color, children,
        priority, rotation)
    {
    }
    
    public Item Item;
    public bool IsDropTarget => true;

    public override void AfterDraw(float deltaTime, SpriteBatch batch)
    {
        if(Item != null)
            batch.Draw(Item.Reference.sprite, rect, Color.White);
    }

    public bool OnDrag()
    {
        PointerItemRenderer.HeldItem = Item;
        return Item != null;
    }

    public void OnDragCancelled()
    {
        PointerItemRenderer.HeldItem = null;
    }

    public bool OnDrop<T>(T genericSource)
    {
        if (genericSource is ItemSlot source)
        {
            PointerItemRenderer.HeldItem = null;
            //Swap items
            (source.Item, Item) = (Item, source.Item);
            //Always accept
            return true;
        }
        return false;
    }
}

public static class PointerItemRenderer
{
    public static Item HeldItem;
    static PointerItemRenderer()
    {
        MainGame.OnDrawUI += DrawHeldItem;
    }

    private static void DrawHeldItem(float deltaTime, SpriteBatch batch)
    {
        if (HeldItem == null) return;
        var sprite = HeldItem.Reference.sprite;
        batch.Draw(sprite, new Rectangle(Input.MousePositionWithinViewport, new Point(sprite.Width, sprite.Height)), Color.White);
    }
}