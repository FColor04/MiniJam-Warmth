using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MiniJam_Warmth;

/// <summary>
/// Contains data about this / last frame input states for different devices
/// </summary>
public static class Input
{
    public static MouseState MouseState;
    public static GamePadState GamepadState;
    public static KeyboardState KeyboardState;

    public static MouseState PreviousMouseState;
    public static GamePadState PreviousGamepadState;
    public static KeyboardState PreviousKeyboardState;
    
    public static Point MousePosition => MouseState.Position;
    /// <summary>
    /// Returns a mouse position relative to window size, where 0 means top left corner and 1 means bottom right
    /// <remarks>It's not clamped between 0 and 1</remarks>
    /// </summary>
    public static Vector2 NormalizedMousePosition => MouseState.Position.ToVector2() / MainGame.WindowSize.ToVector2();
    public static bool Exit => KeyboardState.IsKeyDown(Keys.Escape) || GamepadState.Buttons.Start == ButtonState.Pressed;
    public static bool ToggleFullscreen => Keys.F11.WasPressedThisFrame();

    public static void UpdateState()
    {
        PreviousMouseState = MouseState;
        PreviousGamepadState = GamepadState;
        PreviousKeyboardState = KeyboardState;
        
        MouseState = Mouse.GetState();
        GamepadState = GamePad.GetState(PlayerIndex.One);
        KeyboardState = Keyboard.GetState();
    }

    /// <summary>
    /// Checks if key was pressed this frame
    /// </summary>
    /// <param name="key">Key to check</param>
    /// <returns>Was pressed this frame</returns>
    public static bool WasPressedThisFrame(this Keys key)
    {
        return KeyboardState.IsKeyDown(key) && !PreviousKeyboardState.IsKeyDown(key);
    }
    
    /// <summary>
    /// Checks if key was released this frame
    /// </summary>
    /// <param name="key">Key to check</param>
    /// <returns>Was released this frame</returns>
    public static bool WasReleasedThisFrame(this Keys key)
    {
        return !KeyboardState.IsKeyDown(key) && PreviousKeyboardState.IsKeyDown(key);
    }
    
    /// <summary>
    /// Checks if key is held this frame
    /// </summary>
    /// <param name="key">Key to check</param>
    /// <returns>Is being held this frame (false if just pressed)</returns>
    public static bool IsHeldThisFrame(this Keys key)
    {
        return KeyboardState.IsKeyDown(key) && PreviousKeyboardState.IsKeyDown(key);
    }
}