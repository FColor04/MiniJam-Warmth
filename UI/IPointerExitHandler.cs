using Microsoft.Xna.Framework;

namespace ReFactory;

public interface IPointerExitHandler : IHasInteractiveRect
{
    public void OnPointerExit();
}