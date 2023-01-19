using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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

    private GraphicsDeviceManager _graphics;
    public static Point WindowSize => new Point(Instance._graphics.PreferredBackBufferWidth, Instance._graphics.PreferredBackBufferHeight);
    private SpriteBatch _spriteBatch;
    
    public MainGame()
    {
        Instance = this;
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        Window.AllowUserResizing = true;
    }

    protected override void Initialize()
    {
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
    }

    protected override void Update(GameTime gameTime)
    {
        float deltaTime = (float) gameTime.ElapsedGameTime.TotalSeconds;
        
        //Input is exception so it's executed before other stuff not to cause race conditions.
        Input.UpdateState();
        if (Input.Exit)
            Exit();
        
        //Everything else should subscribe to OnUpdate event
        OnUpdate?.Invoke(deltaTime);

        base.Update(gameTime);
    }

    private readonly Matrix _spriteBatchMatrix = Matrix.Add(Matrix.CreateScale(2), Matrix.CreateTranslation(32, 0, 0));

    protected override void Draw(GameTime gameTime)
    {
        float deltaTime = (float) gameTime.ElapsedGameTime.TotalSeconds;
        GraphicsDevice.Clear(Color.CornflowerBlue);

        OnDraw?.Invoke(deltaTime);
        
        _spriteBatch.Begin(SpriteSortMode.BackToFront, samplerState: SamplerState.PointClamp, transformMatrix: _spriteBatchMatrix);
        
        OnDrawSprites?.Invoke(deltaTime, _spriteBatch);
        
        _spriteBatch.End();
        base.Draw(gameTime);
    }
}