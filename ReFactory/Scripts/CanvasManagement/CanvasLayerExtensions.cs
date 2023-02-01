namespace CanvasManagement;

public static class CanvasLayerExtensions
{
    public static Canvas GetCanvas(this CanvasLayer layer) => CanvasManager.Canvases[layer];
}