using System;
using CanvasManagement;
using FontStashSharp;
using MainGameFramework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReFactory.GameScripts;

namespace ReFactory.UISystem;

public static class PointerItemRenderer
{
    public static Func<Item> GetItem = () => null;
    public static Action<Item> SetItem = _ => {};
    public static Item HeldItem => GetItem();
    static PointerItemRenderer()
    {
        CanvasLayer.UI.GetCanvas().OnDraw += DrawHeldItem;
    }

    private static void DrawHeldItem(SpriteBatch spriteBatch, Canvas canvas)
    {
        if (HeldItem?.Count <= 0) 
            SetItem(null);
        if (HeldItem == null) return;
        var sprite = HeldItem.Reference.sprite;
        spriteBatch.Draw(sprite, new Rectangle(canvas.MousePosition.ToPoint(), new Point(sprite.Width, sprite.Height)), Color.White);
        spriteBatch.DrawString(GameContent.Font11,
            $"{HeldItem.Count}",
            canvas.MousePosition,
            Color.White);
    }
}