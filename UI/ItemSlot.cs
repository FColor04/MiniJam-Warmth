using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReFactory.GameScripts;
using FontStashSharp;
namespace ReFactory;

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
    
    private float timer = 1;
    
    public override void AfterDraw(float deltaTime, SpriteBatch batch)
    {
        timer += deltaTime;

        timer %= 32;
        
        if (Item != null)
        {
            batch.Draw(Item.Reference.sprite, rect, Color.White);
            batch.DrawString(GameContent.Font11,
                $"{Math.Min(Item.Count, 99)}{(Item.Count > 99 ? "+" : "")}",
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
                PointerItemRenderer.SetItem(Item);
                Item = tempItem;
                PointerItemRenderer.GetItem = () => null;
                PointerItemRenderer.SetItem = _ => {};
            }
            else
            {
                PointerItemRenderer.GetItem = () => Item;
                PointerItemRenderer.SetItem = newItem => Item = newItem;
            }
        }
    }
}