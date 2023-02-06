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
        public static MouseState mouseState;
        public static GamePadState gamepadState;
        public static KeyboardState keyboardState;

        public static MouseState previousMouseState;
        public static GamePadState previousGamepadState;
        public static KeyboardState previousKeyboardState;

        public static Point MousePosition => mouseState.Position;
        public static bool Exit => keyboardState.IsKeyDown(Keys.Escape) || gamepadState.Buttons.Start == ButtonState.Pressed;
        public static bool ToggleFullscreen => Keys.F11.WasPressedThisFrame();

        public static bool MiddleMousePressed => mouseState.MiddleButton == ButtonState.Pressed && previousMouseState.MiddleButton == ButtonState.Released;
        public static bool RightMousePressed => mouseState.RightButton == ButtonState.Pressed && previousMouseState.RightButton == ButtonState.Released;
        public static bool LeftMousePressed => mouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released;
        
        public static bool LeftMouseHold => mouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Pressed;
        public static bool RightMouseHold => mouseState.RightButton == ButtonState.Pressed && previousMouseState.RightButton == ButtonState.Pressed;
        
        public static bool LeftMouseRelease => mouseState.LeftButton == ButtonState.Released && previousMouseState.LeftButton == ButtonState.Pressed;
        public static bool RightMouseRelease => mouseState.RightButton == ButtonState.Released && previousMouseState.RightButton == ButtonState.Pressed;
        public static bool MiddleMouseRelease => mouseState.MiddleButton == ButtonState.Released && previousMouseState.MiddleButton == ButtonState.Pressed;

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
            previousMouseState = mouseState;
            previousGamepadState = gamepadState;
            previousKeyboardState = keyboardState;

            mouseState = Mouse.GetState();
            gamepadState = GamePad.GetState(PlayerIndex.One);
            keyboardState = Keyboard.GetState();
        }

        /// <summary>
        /// Checks if key was pressed this frame
        /// </summary>
        /// <param name="key">Key to check</param>
        /// <returns>Was pressed this frame</returns>
        public static bool WasPressedThisFrame(this Keys key)
        {
            return keyboardState.IsKeyDown(key) && !previousKeyboardState.IsKeyDown(key);
        }
        
        /// <summary>
        /// Checks if key is pressed this frame
        /// </summary>
        /// <param name="key">Key to check</param>
        /// <returns>Is pressed this frame</returns>
        public static bool IsPressedThisFrame(this Keys key)
        {
            return keyboardState.IsKeyDown(key);
        }

        /// <summary>
        /// Checks if key was released this frame
        /// </summary>
        /// <param name="key">Key to check</param>
        /// <returns>Was released this frame</returns>
        public static bool WasReleasedThisFrame(this Keys key)
        {
            return !keyboardState.IsKeyDown(key) && previousKeyboardState.IsKeyDown(key);
        }

        /// <summary>
        /// Checks if key is held this frame
        /// </summary>
        /// <param name="key">Key to check</param>
        /// <returns>Is being held this frame (false if just pressed)</returns>
        public static bool IsHeldThisFrame(this Keys key)
        {
            return keyboardState.IsKeyDown(key) && previousKeyboardState.IsKeyDown(key);
        }
    }
}