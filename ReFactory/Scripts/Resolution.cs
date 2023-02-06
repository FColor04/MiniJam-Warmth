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
        if (MainGame.GraphicsDevice == null) return;

        if (Input.ToggleFullscreen)
        {
            fullscreen = !fullscreen;
            if (fullscreen)
            {
                //Hard set to current resolution, TODO: Replace with current resolution setting
                MainGame.GraphicsDeviceManager.PreferredBackBufferWidth = MainGame.GraphicsDevice.DisplayMode.Width;
                MainGame.GraphicsDeviceManager.PreferredBackBufferHeight = MainGame.GraphicsDevice.DisplayMode.Height;
            }
            else
            {
                MainGame.GraphicsDeviceManager.PreferredBackBufferWidth = MainGame.GraphicsDevice.DisplayMode.Width / 2;
                MainGame.GraphicsDeviceManager.PreferredBackBufferHeight = MainGame.GraphicsDevice.DisplayMode.Height / 2;
            }
            MainGame.GraphicsDeviceManager.IsFullScreen = fullscreen;
            MainGame.GraphicsDeviceManager.ApplyChanges();
        }

        if (MainGame.GraphicsDevice.Viewport.Width == _previousWidth &&
            MainGame.GraphicsDevice.Viewport.Height == _previousHeight)
            return;
        
        _previousWidth = MainGame.GraphicsDevice.Viewport.Width;
        _previousHeight = MainGame.GraphicsDevice.Viewport.Height;
        OnResolutionChange?.Invoke();
    }
}