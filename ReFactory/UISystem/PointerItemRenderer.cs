using System;
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
        MainGame.OnDrawUI += DrawHeldItem;
    }

    private static void DrawHeldItem(float deltaTime, SpriteBatch batch)
    {
        if (HeldItem?.Count <= 0) 
            SetItem(null);
        if (HeldItem == null) return;
        var sprite = HeldItem.Reference.sprite;
        batch.Draw(sprite, new Rectangle(Input.MousePositionWithinViewport, new Point(sprite.Width, sprite.Height)), Color.White);
        batch.DrawString(GameContent.Font11,
            $"{HeldItem.Count}",
            Input.MousePositionWithinViewport.ToVector2(),
            Color.White);
    }
}