using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ReFactory;

public class UIElement : IDisposable
{
    private static int _autoPriority = 0;
    
    public Rectangle rect;
    public Color color;
    public float rotation;

    public virtual Rectangle InteractiveRect => rect;
    public readonly int Priority;
    [CanBeNull] public readonly Texture2D Texture;
    public readonly List<UIElement> Children;

    public UIElement(UI.Margin margin, Texture2D texture = null, Color? color = null,
        List<UIElement> children = null, int priority = -1, float rotation = 0) : this(margin.GetRect, texture, color, children, priority, rotation)
    {
            
    }
    public UIElement(Rectangle? rect = default, Texture2D texture = null, Color? color = null, List<UIElement> children = null, int priority = -1, float rotation = 0)
    {
        rect ??= new Rectangle(Point.Zero, Resolution.gameSize);
        color ??= Color.White;
            
        this.rect = rect.Value;
        this.color = color.Value;
            
        Texture = texture;
        Children = children ?? new List<UIElement>();
        Priority = priority == -1 ? _autoPriority : priority;
        _autoPriority++;
    }
    
    public void ProcessUsingLayoutController(UILayoutController controller)
    {
        for (int i = 0; i < Children.Count; i++)
        {
            Children[i].SetRect(controller.GetChildRect(Children[i], i, Children.Count));
        }
    }

    private void SetRect(Rectangle newRect)
    {
        rect = newRect;
    }

    public virtual void AfterDraw(float deltaTime, SpriteBatch batch) {}

    public void AddChild(UIElement element) => Children.Add(element);

    public void Dispose()
    {
        foreach (var uiElement in UI.Root.Flatten())
        {
            uiElement.Children.Remove(this);
        }
        foreach (var uiElement in Children)
        {
            uiElement.Dispose();
        }
        GC.SuppressFinalize(this);
    }
}