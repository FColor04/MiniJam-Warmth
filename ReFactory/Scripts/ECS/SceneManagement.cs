using System;
using System.Collections.Generic;
using ECS.Components;
using ECS.Systems;
using MainGameFramework;
using Microsoft.Xna.Framework.Graphics;
using ReFactory;

namespace ECS;

public static class SceneManagement
{
    public static WeakReference<Scene> currentScene;
    private static Scene _gameScene;
    
    private static RigidbodySystem _rigidbodySystem = new ();
    private static SpriteSystem _spriteSystem = new ();
    
    static SceneManagement()
    {
        _gameScene = new Scene();
        currentScene = new WeakReference<Scene>(_gameScene, false);

        _gameScene
            .AddEntity()
            .AddComponent(new SpriteRenderer()
            {
                texture = GameContent.Rocks
            });
        
        Debug.Log($"Created main scene with {_gameScene.entities.Count} entities");
        currentScene = new WeakReference<Scene>(_gameScene);
    }
}