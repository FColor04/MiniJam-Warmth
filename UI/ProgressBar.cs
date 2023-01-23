using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MiniJam_Warmth;

public class ProgressBar : UIElement
{
    public Color fillColor;
    public float fillAmount;
    
    public ProgressBar(Rectangle? rect, Color backgroundColor, Color fillColor, Texture2D texture = null, List<UIElement> children = null, int priority = -1, float rotation = 0) 
        : base(rect, texture, backgroundColor, children, priority, rotation)
    {
        this.fillColor = fillColor;
    }

    public override void AfterDraw(float deltaTime, SpriteBatch batch)
    {
        var fillRect = new Rectangle(rect.Location, new Point((int) (rect.Width * fillAmount), rect.Height));
        batch.Draw(UI.Pixel, fillRect, fillColor);
    }
}