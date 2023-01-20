using Microsoft.Xna.Framework;

namespace MiniJam_Warmth;

public interface IPointerExitHandler : IHasInteractiveRect
{
    public void OnPointerExit();
}