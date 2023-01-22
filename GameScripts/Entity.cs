﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MiniJam_Warmth.GameScripts;

public class Entity : IDisposable
{
    public Vector2 position;
    protected virtual Vector2 _position => position - MainGame.Instance.World.cameraOffset;
    public virtual Texture2D sprite => null;
    public virtual Vector2 origin => new Vector2(sprite.Width / 2f, sprite.Height / 2f);

    public Entity()
    {
        MainGame.OnDrawSprites += DrawSprite;
    }

    ~Entity()
    {
        MainGame.OnDrawSprites -= DrawSprite;
    }
    
    public void Dispose()
    {
        MainGame.OnDrawSprites -= DrawSprite;
        GC.SuppressFinalize(this);
    }

    protected virtual void DrawSprite(float deltaTime, SpriteBatch batch)
    {
        if (sprite == null) return;
        batch.Draw(sprite, new Rectangle(_position.ToPoint(), new Point(sprite.Width, sprite.Height)), Color.White);
    }
}