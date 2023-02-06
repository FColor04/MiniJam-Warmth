using ECS.Systems;
using Microsoft.Xna.Framework;

namespace ECS.Components;

public class Rigidbody : Component
{
    public Rigidbody()
    {
        RigidbodySystem.Register(this);
    }
    
    public Vector2 velocity;
    public float drag = 0;
}