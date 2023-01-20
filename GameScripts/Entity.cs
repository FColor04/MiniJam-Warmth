using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MiniJam_Warmth.GameScripts;

public class Entity
{
    public Vector2 position;
    public virtual Texture2D sprite => null;
    public virtual Vector2 origin => new Vector2(sprite.Width / 2f, sprite.Height / 2f);

    public Entity()
    {
        MainGame.OnDrawSprites += DrawSprite;
    }

    private void DrawSprite(float deltaTime, SpriteBatch batch)
    {
        batch.Draw(sprite, position, null, Color.White, 0, origin, new Vector2(sprite.Width, sprite.Height), SpriteEffects.None, 0);
    }
}