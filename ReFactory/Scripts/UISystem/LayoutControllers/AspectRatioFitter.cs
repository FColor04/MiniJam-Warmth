using Microsoft.Xna.Framework;

namespace ReFactory.UISystem.LayoutControllers;

public class AspectRatioFitter : UILayoutController
{
    private float ratio;
    
    public AspectRatioFitter(float heightToWidthRatio)
    {
        ratio = heightToWidthRatio;
    }
    
    public override Rectangle GetChildRect(UIElement child, int index, int childrenCount)
    {
        return child.rect.SetSize(new Point((int) (ratio * child.rect.Height), child.rect.Height));
    }
}