

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ParticleSystemTesting;

namespace ReFactory.ParticleSystem.Particles
{

    public class ps_Particle
    {

        private readonly ps_ParticleData _data;
        private static Vector2 _position;
        private static float _lifespanLeft;
        private static float _lifespanAmount;
        private static Color _color;
        private static float _opacity;
        public static bool IsFinished = false;

        public ps_Particle(Vector2 pos, ps_ParticleData data)
        {
            _data = data;
            _lifespanLeft = data.Lifespan;
            _lifespanAmount = 1f;
            _position = pos;
            _color = data.ColorStart;
            _opacity = _data.OpacityStart;
        }

        public void Update()
        {
            _lifespanLeft -= Time.DeltaTime;
            if (_lifespanLeft <= 0f)
            {
                IsFinished = true;
                return;
            }

            _lifespanAmount = MathHelper.Clamp(_lifespanLeft / _data.Lifespan, 0, 1);
            _color = Color.Lerp(_data.ColorEnd, _data.ColorStart, _lifespanAmount);
            _opacity = MathHelper.Clamp(MathHelper.Lerp(_data.OpacityEnd, _data.OpacityStart, _lifespanAmount), 0, 1);
        }

        public void Draw()
        {
            ps_Globals.SpriteBatch.Draw(_data.Texture, _position, null, _color * _opacity, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 1f);
        }
    }
}