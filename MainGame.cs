using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MiniJam_Warmth.Controllers;

namespace MiniJam_Warmth;

public class MainGame : Game
{
    #region Exposed Actions
    
    //Method like initialized & load content were also exposed,
    // but realistically nothing game related should be instantiated at this point so the methods are redundant
    //If you have need for initialize action, then just call a method from initialize directly.
    
    /// <summary>
    /// Called within <see cref="Update"/>>
    /// </summary>
    public static event Action<float> OnUpdate = _ => {};
    /// <summary>
    /// Called within <see cref="Draw"/>, for usage of main sprite batch use <see cref="OnDrawSprites"/>>
    /// <remarks>This is called before <see cref="OnDrawSprites"/></remarks>
    /// </summary>
    public static event Action<float> OnDraw = _ => {};
    /// <summary>
    /// Called within <see cref="Draw"/>, provides pixel art sprite batch to draw sprites within.
    /// <remarks>This is called after <see cref="OnDraw"/></remarks>
    /// </summary>
    public static event Action<float, SpriteBatch> OnDrawSprites = (_, _) => {};
    #endregion

    public static MainGame Instance;
    public static GraphicsDevice graphicsDevice => Instance.GraphicsDevice;
    private readonly GraphicsDeviceManager _graphics;
    public static GraphicsDeviceManager graphicsDeviceManager => Instance._graphics;
    public static Point WindowSize => new Point(Instance._graphics.PreferredBackBufferWidth, Instance._graphics.PreferredBackBufferHeight);
    private SpriteBatch _spriteBatch;
    private RenderTarget2D _renderTarget;
    private StateMachine playerStateMachine;
    
    public MainGame()
    {
        Instance = this;
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        Window.AllowUserResizing = true;
        Console.WriteLine(Test.test);
    }

    protected override void Initialize()
    {
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _renderTarget = new RenderTarget2D(GraphicsDevice, 320, 180);
    }

    protected override void Update(GameTime gameTime)
    {
        float deltaTime = (float) gameTime.ElapsedGameTime.TotalSeconds;
        
        Time.UnscaledDeltaTime = deltaTime;
        Time.UnscaledTotalTime = (float) gameTime.TotalGameTime.TotalSeconds;
        
        deltaTime *= Time.TimeScale;
        
        Time.TotalTime += deltaTime;
        Time.DeltaTime = deltaTime;

        //Input is exception so it's executed before other stuff not to cause race conditions.
        Input.UpdateState();
        if (Input.Exit)
            Exit();
        
        //Everything else should subscribe to OnUpdate event
        OnUpdate?.Invoke(deltaTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        float deltaTime = (float) gameTime.ElapsedGameTime.TotalSeconds;
        GraphicsDevice.SetRenderTarget(_renderTarget);
        GraphicsDevice.Clear(Color.MonoGameOrange);

        OnDraw?.Invoke(deltaTime);
        
        _spriteBatch.Begin(SpriteSortMode.BackToFront, samplerState: SamplerState.PointClamp);
        
        OnDrawSprites?.Invoke(deltaTime, _spriteBatch);
        
        _spriteBatch.End();
        
        GraphicsDevice.SetRenderTarget(null);
        GraphicsDevice.Clear(Color.Black);
        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        _spriteBatch.Draw(_renderTarget, Resolution.TrimmedScreen, Color.White);
        _spriteBatch.End();
        
        base.Draw(gameTime);
    }
}