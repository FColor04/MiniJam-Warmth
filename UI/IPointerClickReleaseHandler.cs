namespace MiniJam_Warmth;

public interface IPointerClickReleaseHandler : IHasInteractiveRect
{
    /// <summary>
    /// When pointer releases the object
    /// </summary>
    /// <param name="buttonIndex">0 - Left mouse button, 1 - Right mouse button, 2 - Middle mouse button</param>
    public void OnPointerClickRelease(int buttonIndex);
}