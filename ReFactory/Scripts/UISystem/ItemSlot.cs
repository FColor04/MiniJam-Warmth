using System;
using System.Collections.Generic;
using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReFactory.GameScripts;

namespace ReFactory.UISystem;

public class ItemSlot : UIElement, IPointerClickHandler
{
    public ItemSlot(Func<Item> getter, Action<Item> setter, Rectangle? rect = default, Texture2D texture = null, Color? color = null,
        List<UIElement> children = null, int priority = -1, float rotation = 0) : base(rect, texture, color, children,
        priority, rotation)
    {
        _getItem = getter;
        _setItem = setter;
    }

    private readonly Func<Item> _getItem;
    private readonly Action<Item> _setItem;
    public Item Item
    {
        get => _getItem?.Invoke();
        set => _setItem?.Invoke(value);
    } 
    public bool IsDropTarget => true;
    
    public override void AfterDraw(SpriteBatch batch)
    {
        if (Item != null)
        {
            batch.Draw(Item.Reference.sprite, rect, Color.White);
            batch.DrawString(GameContent.Font11,
                $"{Math.Min(Item.count, 99)}{(Item.count > 99 ? "+" : "")}",
                rect.Location.ToVector2(),
                Color.White);
        }
    }

    public void OnPointerClick(int buttonIndex)
    {
        if (buttonIndex == 0)
        {
            var tempItem = PointerItemRenderer.HeldItem;
            if (tempItem != null)
            {
                PointerItemRenderer.setItem(Item);
                Item = tempItem;
                PointerItemRenderer.getItem = () => null;
                PointerItemRenderer.setItem = _ => {};
            }
            else
            {
                PointerItemRenderer.getItem = () => Item;
                PointerItemRenderer.setItem = newItem => Item = newItem;
            }
        }
    }
}