using CanvasManagement;
using MainGameFramework;
using ReFactory;

namespace DefaultNamespace;

public static class CameraController
{
    private static int GridSize = 16;
    private static int worldWidth = 32;

    static CameraController()
    {
        MainGame.OnUpdate += CameraMove;
    }
    
    public static void CameraMove(float deltaTime)
    {
        var viewportOffset = CanvasLayer.Base.GetCanvas().ViewportOffset;
        viewportOffset.Y += Input.Vertical * deltaTime * GridSize * 8;
        viewportOffset.X += Input.Horizontal * deltaTime * GridSize * 8;

        if (viewportOffset.X >= ((worldWidth * GridSize) - GridSize) / 2 || viewportOffset.X <= -((worldWidth * GridSize) - GridSize) / 2)
        {
            viewportOffset.X -= Input.Horizontal * deltaTime * GridSize * 8;
        }
        if (viewportOffset.Y >= (((worldWidth * GridSize) - GridSize) * 3) / 4 || viewportOffset.Y <= -((worldWidth * GridSize) - GridSize) / 4)
        {
            viewportOffset.Y -= Input.Vertical * deltaTime * GridSize * 8;
        }

        CanvasLayer.Base.GetCanvas().ViewportOffset = viewportOffset;
    }
}