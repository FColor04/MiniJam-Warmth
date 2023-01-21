namespace MiniJam_Warmth;

public interface IPointerClickHandler : IHasInteractiveRect
{
    /// <summary>
    /// When pointer presses the object
    /// </summary>
    /// <param name="buttonIndex">0 - Left mouse button, 1 - Right mouse button, 2 - Middle mouse button</param>
    public void OnPointerClick(int buttonIndex);
}