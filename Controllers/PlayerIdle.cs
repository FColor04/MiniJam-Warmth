using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MiniJam_Warmth.Controllers;


//This is test state, it's going to be deleted
public class PlayerIdle : IState
{
    private List<Texture2D> _testTextureList = new ();
    private float timer;
    
    public void OnEnter()
    {
        _testTextureList.Clear();
        for (int i = 0; i < 10; i++)
        {
            var texture = new Texture2D(MainGame.graphicsDevice, 1, 1);
            texture.SetData(new []{ Color.Lerp(Color.Red, Color.Blue, i / 10f)});
            _testTextureList.Add(texture);
        }
        MainGame.OnDrawSprites += DrawAnimation;
    }

    public void OnExit()
    {
        MainGame.OnDrawSprites -= DrawAnimation;
    }

    private void DrawAnimation(float deltaTime, SpriteBatch batch)
    {
        batch.Draw(_testTextureList[Convert.ToInt32(Math.Floor(timer % _testTextureList.Count))], new Rectangle(Point.Zero, new Point(256, 256)), Color.White);
        timer += deltaTime * 10f;
    }
}