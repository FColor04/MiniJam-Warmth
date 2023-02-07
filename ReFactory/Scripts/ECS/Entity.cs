using System;
using System.Collections.Generic;
using ECS.Components;
using JetBrains.Annotations;
using Microsoft.Xna.Framework;

namespace ECS;

public class Entity : IDisposable
{
    public Guid id;
    public Scene Scene { get; }
    public readonly List<Component> components;
    public Transform Transform { get; }
    
    public Entity(Scene scene)
    {
        id = Guid.NewGuid();
        components = new List<Component>();
        Scene = scene;
        Transform = new Transform();
        AddComponent(Transform);
    }

    public Entity SetPosition(Vector2 newPosition)
    {
        Transform.position = newPosition;
        return this;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            foreach (var component in components)
            {
                component.Dispose();
            }
            components.Clear();
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public Entity AddComponent(Component component)
    {
        component.entity = this;
        components.Add(component);
        return this;
    }
    
    public Component AddComponent<TComponent>() where TComponent : Component, new()
    {
        var componentInstance = new TComponent();
        AddComponent(componentInstance);
        return componentInstance;
    }

    public int RemoveComponent<TComponent>() where TComponent : Component
    {
        return components.RemoveAll(component =>
        {
            if (component is not TComponent) return false;
            component.Dispose();
            return true;
        });
    }

    public WeakReference<TComponent> GetComponent<TComponent>() where TComponent : Component
    {
        foreach (var component in components)
        {
            if (component is TComponent tComponent)
                return new WeakReference<TComponent>(tComponent, false);
        }

        throw new Exception("Component was not found, to prevent this being an error, use TryGetComponent");
    }
    
    public bool TryGetComponent<TComponent>(out WeakReference<TComponent> tComponent) where TComponent : Component
    {
        tComponent = null;
        foreach (var component in components)
        {
            if (component is TComponent castComponent)
            {
                tComponent = new WeakReference<TComponent>(castComponent, false);
                return true;
            }
        }
        return false;
    }
}