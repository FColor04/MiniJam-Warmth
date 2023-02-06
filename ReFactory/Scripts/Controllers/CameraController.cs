using System;
using CanvasManagement;
using ECS;
using MainGameFramework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ReFactory.Controllers;

public static class CameraController
{
    private static readonly Canvas Canvas;

    static CameraController()
    {
        Canvas = CanvasLayer.Base.GetCanvas();
        MainGame.OnUpdate += Update;
    }

    private static void Update(float deltaTime)
    {
        var horizontal = 0f;
        var vertical = 0f;
        
        if (Keys.A.IsPressedThisFrame())
        {
            horizontal = -1;
        }else if (Keys.D.IsPressedThisFrame())
        {
            horizontal = 1;
        }
        
        if (Keys.W.IsPressedThisFrame())
        {
            vertical = -1;
        }else if (Keys.S.IsPressedThisFrame())
        {
            vertical = 1;
        }

        Canvas.ViewportOffset += new Vector2(horizontal, vertical) * deltaTime * 32;
    }
}