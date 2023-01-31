using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReFactory.ParticleSystem;

namespace ParticleSystemTesting
{
    public struct ps_ParticleData
    {

        private static Texture2D _defaultTexture;
        public Texture2D Texture = _defaultTexture ??= ps_Globals.Content.Load<Texture2D>("MyParticleTextImg");
        public float Lifespan = 2f;
        public Color ColorStart = Color.Yellow;
        public Color ColorEnd = Color.Red;
        public float OpacityStart = 1f;
        public float OpacityEnd = 0f;

        public ps_ParticleData()
        {

        }
    }
}