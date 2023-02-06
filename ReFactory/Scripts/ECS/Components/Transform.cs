using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Utility;

namespace ECS.Components;

public sealed class Transform : Component
{
    public Transform() {}
    public Transform(Vector2 position)
    {
        this.position = position;
    }
    
    public Vector2 position = Vector2.Zero;
    public Vector2 scale = Vector2.One;
    public Rotation rotation = Rotation.Up;
    public List<Entity> children = new ();
}