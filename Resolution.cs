using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MiniJam_Warmth;

public static class Resolution
{
    public static readonly Point gameSize = new Point(320, 180);
    private static int _previousWidth;
    private static int _previousHeight;
    public static Rectangle TrimmedScreen;
    public static bool fullscreen;
    
    static Resolution()
    {
        MainGame.OnUpdate += CheckResolution;
    }

    /// <summary>
    /// Checks if resolution has changed (By User resizing window)
    /// or if fullscreen key is pressed.
    /// </summary>
    /// <param name="deltaTime">Delta time</param>
    private static void CheckResolution(float deltaTime)
    {
        if (MainGame.graphicsDevice == null) return;

        if (Input.ToggleFullscreen)
        {
            fullscreen = !fullscreen;
            if (fullscreen)
            {
                //Hard set to current resolution, TODO: Replace with current resolution setting
                MainGame.graphicsDeviceManager.PreferredBackBufferWidth = MainGame.graphicsDevice.DisplayMode.Width;
                MainGame.graphicsDeviceManager.PreferredBackBufferHeight = MainGame.graphicsDevice.DisplayMode.Height;
            }
            else
            {
                MainGame.graphicsDeviceManager.PreferredBackBufferWidth = MainGame.graphicsDevice.DisplayMode.Width / 2;
                MainGame.graphicsDeviceManager.PreferredBackBufferHeight = MainGame.graphicsDevice.DisplayMode.Height / 2;
            }
            MainGame.graphicsDeviceManager.IsFullScreen = fullscreen;
            MainGame.graphicsDeviceManager.ApplyChanges();
        }

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