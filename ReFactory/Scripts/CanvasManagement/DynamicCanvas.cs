using System;
using MainGameFramework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReFactory;

namespace CanvasManagement;

public class DynamicCanvas : Canvas
{
    public float AspectRatio { get; }
    public bool useFullSize;
    
    public DynamicCanvas(int width, int height, CanvasLayer layer, float aspectRatio) : base(0, 0, 1, 1, layer, width, height)
    {
        if (width == 0 || height == 0)
        {
            useFullSize = true;
        }
        AspectRatio = aspectRatio;
        Resolution.OnResolutionChange += ChangeSize;
        ChangeSize();
    }

    private void ChangeSize()
    {
        var newWidth = Math.Min(Resolution.WindowHeight * AspectRatio, Resolution.WindowWidth);
        var newHeight = newWidth * (1f / AspectRatio);
        var widthDelta = Resolution.WindowWidth - newWidth;
        var heightDelta = Resolution.WindowHeight - newHeight;
        Rect = new Rectangle(new Vector2(widthDelta / 2f, heightDelta / 2f).ToPoint(), new Vector2(newWidth, newHeight).ToPoint());
        
        if (!useFullSize) return;
        renderTarget?.Dispose();
        if (newWidth <= 0 || newHeight <= 0) return;
        renderTarget = new RenderTarget2D(MainGame.GraphicsDevice, (int) newWidth, (int) newHeight, false, MainGame.GraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.Depth24);
    }
}