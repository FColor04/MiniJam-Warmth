using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.Xna.Framework;

namespace ECS;

public sealed class Scene : IDisposable
{
    public Guid id;
    public readonly List<Entity> entities = new ();

    public Scene()
    {
        id = Guid.NewGuid();
    }

    public Entity AddEntity()
    {
        var newEntity = new Entity(this);
        entities.Add(newEntity);
        return newEntity;
    }

    private void Dispose(bool disposing)
    {
        if (disposing)
        {
            entities.Clear();
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}