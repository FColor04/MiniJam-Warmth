using System.Collections.ObjectModel;

namespace CanvasManagement;

public class Canvases : KeyedCollection<CanvasLayer, Canvas>
{
    protected override CanvasLayer GetKeyForItem(Canvas item)
    {
        return item.Layer;
    }
}