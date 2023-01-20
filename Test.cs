using Microsoft.Xna.Framework.Graphics;

namespace MiniJam_Warmth;

public class Test
{
    public Test()
    {
        MainGame.OnDrawSprites += OnDrawSprites;
        MainGame.OnUpdate += OnUpdate;
    }
    ~Test()
    {
        MainGame.OnDrawSprites -= OnDrawSprites;
        MainGame.OnUpdate -= OnUpdate;
    }

    private void OnUpdate(float deltaTime)
    {
        
    }

    private void OnDrawSprites(float deltaTime, SpriteBatch batch)
    {
        
    }
}