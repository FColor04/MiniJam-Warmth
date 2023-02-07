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
    private static float velocity;
    
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

        var moving = horizontal != 0 || vertical != 0;
        var movingDiagonally = horizontal != 0 && vertical != 0;
        
        //Get up to speed in 1 / 8th of a second
        velocity = MathHelper.Lerp(velocity, moving ? 1 : 0, Time.deltaTime * 8f);
        
        if (movingDiagonally)
        {
            horizontal *= 0.707106781f;
            vertical *= 0.707106781f;
        }

        var speed = Time.preciseDeltaTime * velocity * 128.0;
        var movementVector = new Vector2((float) (horizontal * speed), (float) (vertical * speed));
        Canvas.ViewportOffset += movementVector;
    }
}