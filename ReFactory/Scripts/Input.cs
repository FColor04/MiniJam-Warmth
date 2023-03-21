using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MainGameFramework;

namespace ReFactory
{

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
        public static bool Exit => KeyboardState.IsKeyDown(Keys.Escape) || GamepadState.Buttons.Start == ButtonState.Pressed;
        public static bool ToggleFullscreen => Keys.F11.WasPressedThisFrame();

        public static bool MiddleMousePressed => MouseState.MiddleButton == ButtonState.Pressed && PreviousMouseState.MiddleButton == ButtonState.Released;
        public static bool RightMousePressed => MouseState.RightButton == ButtonState.Pressed && PreviousMouseState.RightButton == ButtonState.Released;
        public static bool LeftMousePressed => MouseState.LeftButton == ButtonState.Pressed && PreviousMouseState.LeftButton == ButtonState.Released;
        
        public static bool LeftMouseHold => MouseState.LeftButton == ButtonState.Pressed && PreviousMouseState.LeftButton == ButtonState.Pressed;
        public static bool RightMouseHold => MouseState.RightButton == ButtonState.Pressed && PreviousMouseState.RightButton == ButtonState.Pressed;
        
        public static bool LeftMouseRelease => MouseState.LeftButton == ButtonState.Released && PreviousMouseState.LeftButton == ButtonState.Pressed;
        public static bool RightMouseRelease => MouseState.RightButton == ButtonState.Released && PreviousMouseState.RightButton == ButtonState.Pressed;
        public static bool MiddleMouseRelease => MouseState.MiddleButton == ButtonState.Released && PreviousMouseState.MiddleButton == ButtonState.Pressed;

        public static int GetPressedMouseButton =>
            LeftMousePressed ? 0 :
            (RightMousePressed ? 1
            : (MiddleMousePressed ? 2
                : -1));

        public static int GetReleasedMouseButton => LeftMousePressed ? 0 :
            (RightMousePressed ? 1
                : (MiddleMousePressed ? 2
                    : -1));

        public static float Vertical => Keys.W.IsPressedThisFrame() ? -1 : Keys.S.IsPressedThisFrame() ? 1 : 0;
        public static float Horizontal => Keys.A.IsPressedThisFrame() ? -1 : Keys.D.IsPressedThisFrame() ? 1 : 0;

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
        /// Checks if key is pressed this frame
        /// </summary>
        /// <param name="key">Key to check</param>
        /// <returns>Is pressed this frame</returns>
        public static bool IsPressedThisFrame(this Keys key)
        {
            return KeyboardState.IsKeyDown(key);
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
}