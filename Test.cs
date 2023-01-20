using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MiniJam_Warmth;

public static class Test
{
    public static int test = 0;
    private static Texture2D _tex;
    
    static Test()
    {
        CreateTexture();
        MainGame.OnDrawSprites += OnDrawSprites;
        MainGame.OnUpdate += OnUpdate;
    }

    private static async void CreateTexture()
    {
        while (MainGame.graphicsDevice == null)
            await Task.Delay(1000);
        _tex = new Texture2D(MainGame.graphicsDevice, 1, 1);
        _tex.SetData(new [] {Color.White});
    }

    private static void OnUpdate(float deltaTime)
    {
        
    }

    private static void OnDrawSprites(float deltaTime, SpriteBatch batch)
    {
        if (_tex == null) return;
        batch.Draw(_tex, new Rectangle(new Point((int) (Math.Sin(Time.TotalTime) * 128 + 128), 0), new Point(64, 64)), Color.White);
    }
}