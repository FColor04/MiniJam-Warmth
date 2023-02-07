using System;
using System.Collections.Generic;
using ECS.Components;
using ECS.Systems;
using MainGameFramework;
using Microsoft.Xna.Framework;
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
        _gameScene.AddEntity()
            .SetPosition(new Vector2(0, 100))
            .AddComponent(new SpriteRenderer()
            {
                texture = GameContent.Rocks
            });
        _gameScene.AddEntity()
            .SetPosition(new Vector2(0, 50.5f))
            .AddComponent(new SpriteRenderer()
            {
                texture = GameContent.Rocks
            });
        _gameScene.AddEntity()
            .SetPosition(new Vector2(0, 21.3742001294f))
            .AddComponent(new SpriteRenderer()
            {
                texture = GameContent.Rocks
            });
        
        Debug.Log($"Created main scene with {_gameScene.entities.Count} entities");
        currentScene = new WeakReference<Scene>(_gameScene);
    }
}