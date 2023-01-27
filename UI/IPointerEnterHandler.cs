using Microsoft.Xna.Framework;

namespace ReFactory;

public interface IPointerEnterHandler : IHasInteractiveRect
{
    public void OnPointerEnter();
}