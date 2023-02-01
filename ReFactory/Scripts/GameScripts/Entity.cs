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
    protected virtual Vector2 _position => position - MainGame.Instance.World.cameraOffset;
    public Texture2D internalSprite = null;
    public virtual Texture2D sprite => internalSprite;
    public virtual Vector2 origin => new Vector2(sprite.Width / 2f, sprite.Height / 2f);

    public Entity()
    {
        CanvasLayer.Base.GetCanvas().OnDraw += DrawSprite;
    }

    protected virtual void DrawSprite(SpriteBatch spriteBatch, Canvas canvas)
    {
        if (sprite == null) return;
        spriteBatch.Draw(sprite, new Rectangle(_position.ToPoint(), new Point(sprite.Width, sprite.Height)), Color.White);
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