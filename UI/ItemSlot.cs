using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MiniJam_Warmth.GameScripts;
using MonoGame.Extended.BitmapFonts;

namespace MiniJam_Warmth;

public class ItemSlot : UIElement, IPointerClickHandler
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
    
    private float timer = 1;
    
    public override void AfterDraw(float deltaTime, SpriteBatch batch)
    {
        timer += deltaTime;

        timer %= 32;
        
        if (Item != null)
        {
            batch.Draw(Item.Reference.sprite, rect, Color.White);
            batch.DrawString(MainGame.Instance.Font,
                $"{Item.Count}",
                rect.Center.ToVector2(),
                Color.White,
                0,
                new Vector2(21, 26.5f), new Vector2(16/64f, 16/64f), SpriteEffects.None, 0);
        }
    }

    public void OnPointerClick(int buttonIndex)
    {
        if (buttonIndex == 0)
        {
            (PointerItemRenderer.HeldItem, Item) = (Item, PointerItemRenderer.HeldItem);
        }
    }
}