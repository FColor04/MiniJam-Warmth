using System;

namespace ECS;

public abstract class Component : IDisposable
{
    public string Name => GetType().Name;
    public Entity entity;

    protected virtual void Dispose(bool disposeManaged)
    {
        if (disposeManaged)
        {
            entity = null;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}