using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MiniJam_Warmth.GameScripts;
using MonoGame.Extended.BitmapFonts;
using MainGameFramework;

namespace MiniJam_Warmth;

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
        batch.DrawString(GameContent.Font,
            $"{HeldItem.Count}",
            Input.MousePositionWithinViewport.ToVector2(),
            Color.White,
            0,
            new Vector2(21, 26.5f), new Vector2(16/64f, 16/64f), SpriteEffects.None, 0);
    }
}