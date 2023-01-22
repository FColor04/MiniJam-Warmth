using System;

namespace MiniJam_Warmth;

public interface IDropHandler : IHasInteractiveRect
{
    public Type GetType();

    /// <summary>
    /// Process item drop of <see cref="IDragHandler"/> if it <see cref="IDragHandler.IsDropTarget"/>
    /// </summary>
    /// <param name="genericSource">Dropped item</param>
    /// <param name="genericSource">Where drag began</param>
    /// <returns>Is item a valid drop target</returns>
    public bool OnDrop<T>(T genericSource);
    public void OnPotentialDrop<T>(T genericSource) {}
}