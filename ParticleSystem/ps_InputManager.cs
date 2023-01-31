

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ReFactory.ParticleSystem
{

    public static class ps_InputManager
    {

        private static MouseState _lastMouseState;
        public static bool HasClicked { get; private set; }
        public static Vector2 MousePosition { get; private set; }

        public static void Update()
        {
            var mouseState = Mouse.GetState();

            HasClicked = mouseState.LeftButton == ButtonState.Pressed && _lastMouseState.LeftButton == ButtonState.Released;

            _lastMouseState = mouseState;
        }
    }
}
