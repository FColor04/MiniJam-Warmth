using System;
using Microsoft.Xna.Framework;

namespace MiniJam_Warmth;

public static class Resolution
{
    private static int _previousWidth;
    private static int _previousHeight;
    public static Rectangle TrimmedScreen;
    
    static Resolution()
    {
        MainGame.OnUpdate += CheckResolution;
    }

    private static void CheckResolution(float deltaTime)
    {
        if (MainGame.graphicsDevice == null) return;
        
        if (MainGame.graphicsDevice.Viewport.Width == _previousWidth &&
            MainGame.graphicsDevice.Viewport.Height == _previousHeight)
            return;
        
        _previousWidth = MainGame.graphicsDevice.Viewport.Width;
        _previousHeight = MainGame.graphicsDevice.Viewport.Height;
        
        var newWidth = Math.Min(MainGame.graphicsDevice.Viewport.Height * (16/9f), MainGame.graphicsDevice.Viewport.Width);
        var newHeight = newWidth * (9 / 16f);
        
        var widthDelta = MainGame.graphicsDevice.Viewport.Width - newWidth;
        var heightDelta = MainGame.graphicsDevice.Viewport.Height - newHeight;
        TrimmedScreen = new Rectangle(new Vector2(widthDelta / 2f, heightDelta / 2f).ToPoint(), new Vector2(newWidth, newHeight).ToPoint());
    }
}