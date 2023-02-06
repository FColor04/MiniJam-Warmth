using System;
using System.Collections.Generic;
using CanvasManagement;
using JetBrains.Annotations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReFactory.UISystem.LayoutControllers;

namespace ReFactory.UISystem;

public class UIElement : IDisposable
{
    private static int _autoPriority = 0;
    
    public Rectangle rect;
    public Color color;
    public float rotation;

    public virtual Rectangle InteractiveRect => rect;
    public readonly int priority;
    [CanBeNull] public readonly Texture2D texture;
    public readonly List<UIElement> children;
    
    public UIElement(Rectangle? rect = default, Texture2D texture = null, Color? color = null, List<UIElement> children = null, int priority = -1, float rotation = 0)
    {
        rect ??= new Rectangle(Point.Zero, CanvasLayer.UI.GetCanvas().Size);
        color ??= Color.White;
            
        this.rect = rect.Value;
        this.color = color.Value;
            
        this.texture = texture;
        this.children = children ?? new List<UIElement>();
        this.priority = priority == -1 ? _autoPriority : priority;
        _autoPriority++;
    }
    
    public void ProcessUsingLayoutController(UILayoutController controller)
    {
        for (int i = 0; i < children.Count; i++)
        {
            children[i].SetRect(controller.GetChildRect(children[i], i, children.Count));
        }
    }

    private void SetRect(Rectangle newRect)
    {
        rect = newRect;
    }

    public virtual void AfterDraw(SpriteBatch batch) {}

    public void AddChild(UIElement element) => children.Add(element);

    public void Dispose()
    {
        foreach (var uiElement in UI.Root.Flatten())
        {
            uiElement.children.Remove(this);
        }
        foreach (var uiElement in children)
        {
            uiElement.Dispose();
        }
        GC.SuppressFinalize(this);
    }
}