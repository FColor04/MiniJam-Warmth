using MainGameFramework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReFactory.ParticleSystem;

namespace ParticleSystemTesting
{
    public struct ParticleData
    {
        private static readonly Texture2D DefaultTexture = MainGame.Content.Load<Texture2D>("rocks");
        public readonly Texture2D texture = DefaultTexture;
        public readonly float lifespan = 2f;
        public readonly Color colorStart = Color.Yellow;
        public readonly Color colorEnd = Color.Red;
        public readonly float opacityStart = 1f;
        public readonly float opacityEnd = 0f;

        public ParticleData()
        {

        }
    }
}