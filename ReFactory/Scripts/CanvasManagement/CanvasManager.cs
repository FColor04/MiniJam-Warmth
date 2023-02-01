using MainGameFramework;

namespace CanvasManagement;

public static class CanvasManager
{
    public static readonly Canvases Canvases = new ();

    static CanvasManager()
    {
        Canvases.Add(new DynamicCanvas(320, 180, CanvasLayer.Base, 16/9f));
        Canvases.Add(new DynamicCanvas(320, 180, CanvasLayer.UI, 16/9f));
    }
}