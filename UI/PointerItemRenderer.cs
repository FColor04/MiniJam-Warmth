using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MiniJam_Warmth.GameScripts;
using MonoGame.Extended.BitmapFonts;

namespace MiniJam_Warmth;

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
        batch.DrawString(MainGame.Instance.Font,
            $"{HeldItem.Count}",
            Input.MousePositionWithinViewport.ToVector2(),
            Color.White,
            0,
            new Vector2(21, 26.5f), new Vector2(16/64f, 16/64f), SpriteEffects.None, 0);
    }
}