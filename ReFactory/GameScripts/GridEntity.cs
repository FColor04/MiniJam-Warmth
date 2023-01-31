using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ReFactory.GameScripts;

public abstract class GridEntity : Entity
{
    public virtual float DestroyTime => 1;
    public virtual Func<bool> OnDestroy => () => true;
    public float rotation;
    protected GridEntity() : base()
    {
        
    }

    public abstract HashSet<Point> OccupiedRelativePoints { get; }

    protected override void DrawSprite(float deltaTime, SpriteBatch batch)
    {
        if (sprite == null) return;
        var halfSize = new Vector2(sprite.Width / 2f, sprite.Height / 2f);
        batch.Draw(sprite, new Rectangle(_position.ToPoint() + halfSize.ToPoint(), new Point(sprite.Width, sprite.Height)), null, Color.White, MathHelper.ToRadians(rotation), halfSize, SpriteEffects.None, 0);
    }
}