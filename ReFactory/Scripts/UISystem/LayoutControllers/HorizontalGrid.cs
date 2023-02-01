using Microsoft.Xna.Framework;

namespace ReFactory.UISystem.LayoutControllers;

public class HorizontalGrid : UILayoutController
{
    private UIElement parent;

    public HorizontalGrid(UIElement parent)
    {
        this.parent = parent;
    }

    public override Rectangle GetChildRect(UIElement child, int index, int childrenCount)
    {
        var size = new Point((int)(parent.rect.Width / (float) childrenCount), parent.rect.Height);
        return new Rectangle((size.ToVector2() * new Vector2(index, 0)).ToPoint() + parent.rect.Location, size);
    }
}