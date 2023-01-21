using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace MiniJam_Warmth
{

    /// <summary>
    /// Contains data about this / last frame input states for different devices
    /// </summary>
    public static class Input
    {
        public static MouseState MouseState;
        public static Point MousePosition => MouseState.Position;
        /// <summary>
        /// Returns a mouse position relative to window size, where 0 means top left corner and 1 means bottom right
        /// <remarks>It's not clamped between 0 and 1</remarks>
        /// </summary>
        public static Vector2 NormalizedMousePosition => MouseState.Position.ToVector2() / MainGame.WindowSize.ToVector2();
        public static bool Exit => KeyboardState.IsKeyDown(Keys.Escape) || GamepadState.Buttons.Start == ButtonState.Pressed;

        public static GamePadState GamepadState;
        public static KeyboardState KeyboardState;

        public static void UpdateState()
        {
            MouseState = Mouse.GetState();
            GamepadState = GamePad.GetState(PlayerIndex.One);
            KeyboardState = Keyboard.GetState();
        }

        internal static bool IsKeyPressed(Keys space)
        {
            throw new NotImplementedException();
        }
    }
}