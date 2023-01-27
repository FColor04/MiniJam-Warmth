using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MainGameFramework;

namespace ReFactory;

public class Button : UIElement, IPointerEnterHandler, IPointerExitHandler
{
    private readonly Rectangle _initialRect;
    private Rectangle _targetRect;
    private float transition = 0;
    
    public Button(UI.Margin margin, Texture2D texture = null, Color? color = null,
        List<UIElement> children = null, int priority = -1, float rotation = 0) : this(margin.GetRect, texture, color, children, priority, rotation) { }

    public Button(Rectangle? rect = default, Texture2D texture = null, Color? color = null,
        List<UIElement> children = null, int priority = -1, float rotation = 0) : base(rect, texture, color, children,
        priority, rotation)
    {
        MainGame.OnUpdate += Update;
        _initialRect = rect ?? default;
        _targetRect = rect ?? default;
    }
    
    ~Button()
    {
        MainGame.OnUpdate -= Update;
    }

    private void Update(float deltaTime)
    {
        rect = _targetRect;
    }

    public void OnPointerEnter()
    {
        _targetRect = _initialRect.SetScale(1.1f);
    }
    
    public void OnPointerExit()
    {
        _targetRect = _initialRect;
    }
}