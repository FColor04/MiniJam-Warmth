

using CanvasManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ParticleSystemTesting;

namespace ReFactory.ParticleSystem.Particles
{

    public class Particle
    {

        private readonly ParticleData _data;
        private Vector2 _position;
        private float _lifespanLeft;
        private float _lifespanAmount;
        private Color _color;
        private float _opacity;
        public bool isFinished = false;

        public Particle(Vector2 pos, ParticleData data)
        {
            _data = data;
            _lifespanLeft = data.lifespan;
            _lifespanAmount = 1f;
            _position = pos;
            _color = data.colorStart;
            _opacity = _data.opacityStart;
            CanvasLayer.Base.GetCanvas().OnDraw += Draw;
        }

        public void Update()
        {
            _lifespanLeft -= Time.deltaTime;
            if (_lifespanLeft <= 0f)
            {
                isFinished = true;
                CanvasLayer.Base.GetCanvas().OnDraw -= Draw;
                return;
            }

            _lifespanAmount = MathHelper.Clamp(_lifespanLeft / _data.lifespan, 0, 1);
            _color = Color.Lerp(_data.colorEnd, _data.colorStart, _lifespanAmount);
            _opacity = MathHelper.Clamp(MathHelper.Lerp(_data.opacityEnd, _data.opacityStart, _lifespanAmount), 0, 1);
        }

        public void Draw(SpriteBatch spriteBatch, Canvas canvas)
        {
            spriteBatch.Draw(_data.texture, _position - canvas.ViewportOffset, null, _color * _opacity, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 1f);
        }
    }
}