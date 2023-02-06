using System;
using System.Collections.Generic;
using System.Linq;
using CanvasManagement;
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

    protected override void DrawSprite(SpriteBatch batch, Canvas canvas)
    {
        if (Sprite == null) return;
        var halfSize = new Vector2(Sprite.Width / 2f, Sprite.Height / 2f);
        batch.Draw(Sprite, new Rectangle(Position.ToPoint() + halfSize.ToPoint() - canvas.ViewportOffset.ToPoint(), new Point(Sprite.Width, Sprite.Height)), null, Color.White, MathHelper.ToRadians(rotation), halfSize, SpriteEffects.None, 0);
    }
}