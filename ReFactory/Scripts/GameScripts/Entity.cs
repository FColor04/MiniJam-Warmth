using System;
using System.Diagnostics;
using CanvasManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MainGameFramework;
using Debug = ReFactory.Debugger.Debug;

namespace ReFactory.GameScripts;

public class Entity : IDisposable
{
    public Vector2 position;
    protected virtual Vector2 Position => position;
    public Texture2D internalSprite = null;
    public virtual Texture2D Sprite => internalSprite;
    public virtual Vector2 Origin => new Vector2(Sprite.Width / 2f, Sprite.Height / 2f);

    public Entity()
    {
        CanvasLayer.Base.GetCanvas().OnDraw += DrawSprite;
    }

    protected virtual void DrawSprite(SpriteBatch spriteBatch, Canvas canvas)
    {
        if (Sprite == null) return;
        spriteBatch.Draw(Sprite, new Rectangle(Position.ToPoint() - canvas.ViewportOffset.ToPoint(), new Point(Sprite.Width, Sprite.Height)), Color.White);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposing) return;
        CanvasLayer.Base.GetCanvas().OnDraw -= DrawSprite;
    }

    public void Dispose()
    {
        Debug.Log("Disposing");
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}