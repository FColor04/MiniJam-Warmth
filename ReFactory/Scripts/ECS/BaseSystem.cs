using System;
using System.Collections.Generic;

namespace ECS;

public class BaseSystem<TComponent> where TComponent : Component
{
    protected static readonly List<WeakReference<TComponent>> ComponentReferences = new ();
    public static void Register(TComponent component) => ComponentReferences.Add(new WeakReference<TComponent>(component));
}