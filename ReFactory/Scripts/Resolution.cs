using System;
using Microsoft.Xna.Framework;
using MainGameFramework;

namespace ReFactory;

public static class Resolution
{
    private static int _previousWidth;
    private static int _previousHeight;
    public static int WindowWidth => _previousWidth;
    public static int WindowHeight => _previousHeight;
    public static Point WindowSize => new (WindowWidth, WindowHeight);
    
    public static bool fullscreen;
    public static event Action OnResolutionChange = () => {};
    
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
        OnResolutionChange?.Invoke();
    }
}