using Microsoft.Xna.Framework;

namespace ReFactory;

public class HorizontalGrid : UILayoutController
{
    private UIElement parent;
    private UI.Margin margin;
    public HorizontalGrid(UIElement parent, UI.Margin margin = new UI.Margin())
    {
        this.parent = parent;
        this.margin = margin;
    }
    
    public override Rectangle GetChildRect(UIElement child, int index, int childrenCount)
    {
        var size = new Point((int)(parent.rect.Width / (float) childrenCount), parent.rect.Height);
        return new Rectangle((size.ToVector2() * new Vector2(index, 0) + new Vector2(margin.Left, margin.Top)).ToPoint() + parent.rect.Location, size - new Point(margin.Right + margin.Left, margin.Bottom + margin.Top));
    }
}