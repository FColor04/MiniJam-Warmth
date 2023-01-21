using Microsoft.Xna.Framework;

namespace MiniJam_Warmth;

public interface IPointerEnterHandler : IHasInteractiveRect
{
    public void OnPointerEnter();
}