using ECS.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ECS.Components;

public class SpriteRenderer : Component
{
    public Texture2D texture;
    public Color color = Color.White;
    public Vector2 origin = Vector2.One * 0.5f;
    public SpriteEffects spriteEffects;
    public float layerDepth;

    public SpriteRenderer()
    {
        SpriteSystem.Register(this);
    }
}