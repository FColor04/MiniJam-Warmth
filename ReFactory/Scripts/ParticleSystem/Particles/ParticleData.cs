using MainGameFramework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReFactory.ParticleSystem;

namespace ParticleSystemTesting
{
    public struct ParticleData
    {
        private static readonly Texture2D DefaultTexture = MainGame.content.Load<Texture2D>("rocks");
        public readonly Texture2D Texture = DefaultTexture;
        public readonly float Lifespan = 2f;
        public readonly Color ColorStart = Color.Yellow;
        public readonly Color ColorEnd = Color.Red;
        public readonly float OpacityStart = 1f;
        public readonly float OpacityEnd = 0f;

        public ParticleData()
        {

        }
    }
}