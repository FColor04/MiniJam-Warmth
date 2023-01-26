using Microsoft.Xna.Framework;

namespace ReFactory;

public class FixedSize : UILayoutController
{
    private Point size;
    
    public FixedSize(int width, int height)
    {
        size = new Point(width, height);
    }
    
    public override Rectangle GetChildRect(UIElement child, int index, int childrenCount)
    {
        return child.rect.SetSize(size);
    }
}