using JetBrains.Annotations;
using Microsoft.Xna.Framework;

namespace ReFactory;

public abstract class UILayoutController
{
    public UIElement parent;
    
    /// <summary>
    /// This method controls the children layout
    /// </summary>
    /// <param name="parent">Parent element</param>
    /// <param name="child">Processed Child element</param>
    /// <param name="index">Processed Child index</param>
    /// <param name="childrenCount">Total Children Count</param>
    /// <returns>Child rectangle</returns>
    [Pure]
    public abstract Rectangle GetChildRect(UIElement child, int index, int childrenCount);
}