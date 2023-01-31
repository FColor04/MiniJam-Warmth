using System;
using System.Diagnostics;
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
        MainGame.OnDrawSprites += DrawSprite;
    }

    protected virtual void DrawSprite(float deltaTime, SpriteBatch batch)
    {
        if (sprite == null) return;
        batch.Draw(sprite, new Rectangle(_position.ToPoint(), new Point(sprite.Width, sprite.Height)), Color.White);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposing) return;
        MainGame.OnDrawSprites -= DrawSprite;
    }

    public void Dispose()
    {
        Debug.Log("Disposing");
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}