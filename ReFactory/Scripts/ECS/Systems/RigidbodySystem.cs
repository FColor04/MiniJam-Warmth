using ECS.Components;
using MainGameFramework;

namespace ECS.Systems;

public class RigidbodySystem : BaseSystem<Rigidbody>
{
    public RigidbodySystem()
    {
        MainGame.OnUpdate += Update;
    }

    private void Update(float deltaTime)
    {
        foreach (var component in ComponentReferences)
        {
            if (!component.TryGetTarget(out Rigidbody rigidbody)) continue;
            rigidbody.entity.Transform.position += rigidbody.velocity * deltaTime;
            rigidbody.velocity *= (1 - rigidbody.drag);
        }
        ComponentReferences.RemoveAll(weakReference => weakReference == null);
    }
}