using System;
using CanvasManagement;
using ECS.Components;
using MainGameFramework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReFactory;

namespace ECS.Systems;

public class SpriteSystem : BaseSystem<SpriteRenderer>
{
    private static Canvas _canvas;
    
    public SpriteSystem()
    {
        _canvas = CanvasLayer.Base.GetCanvas();
        _canvas.OnDraw += Draw;
    }

    private static void Draw(SpriteBatch spriteBatch, Canvas canvas)
    {
        foreach (var component in ComponentReferences)
        {
            if (!component.TryGetTarget(out SpriteRenderer spriteRenderer)) continue;
            
            spriteBatch.Draw(
                spriteRenderer.texture,
                (spriteRenderer.entity.Transform.position - _canvas.ViewportOffset.ToPoint().ToVector2()),
                null,
                spriteRenderer.color,
                spriteRenderer.entity.Transform.rotation.Radians,
                spriteRenderer.origin,
                spriteRenderer.entity.Transform.scale,
                spriteRenderer.spriteEffects,
                spriteRenderer.layerDepth
            );
        }

        ComponentReferences.RemoveAll(weakReference => weakReference == null);
    }
}