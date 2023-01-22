/*
 
 Sound Effect from <a href="https://pixabay.com/?utm_source=link-attribution&amp;utm_medium=referral&amp;utm_campaign=music&amp;utm_content=6403">Pixabay</a>
 
 
 
 
 */


using AudioManagementUtil;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MiniJam_Warmth;
using MiniJam_Warmth.Controllers;
using MiniJam_Warmth.GameScripts;
using MiniJam_Warmth.Utility;
using MonoGame.Extended.BitmapFonts;
using ScoreBoardUtil;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using MiniJam_Warmth.GameScripts.Machines;

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
    /// <summary>
    /// Called within <see cref="Draw"/>, provides batch rendered after other batches to draw UI.
    /// <remarks>This is called after <see cref="OnDrawSprites"/></remarks>
    /// </summary>
    public static event Action<float, SpriteBatch> OnDrawUI = (_, _) => { };
    
    #endregion

    public static MainGame Instance;
    public static GraphicsDevice graphicsDevice => Instance.GraphicsDevice;
    private readonly GraphicsDeviceManager _graphics;
    public static GraphicsDeviceManager graphicsDeviceManager => Instance._graphics;
    public static ContentManager content => Instance.Content;
    public static Point WindowSize => new Point(Instance._graphics.PreferredBackBufferWidth, Instance._graphics.PreferredBackBufferHeight);

    private BitmapFont _font;
    public BitmapFont Font
    {
        get
        {
            if(_font == null)
                _font = Content.Load<BitmapFont>("ForwardMini");
            return _font;
        }
    }
    private SpriteBatch _spriteBatch;
    private RenderTarget2D _renderTarget;
    private StateMachine playerStateMachine;
    private AudioManager _audioManager;
    private int WorldSizeX = 17;
    private int WorldSizeY = 17;
    private int RendTargWidth = 320; // Original --> 320 [ by Fcolor04 ]
    private int RendTargHeight = 180; // Original --> 180 [ by Fcolor04 ]
    public static AudioManager AudioManager => Instance._audioManager;
    
    public World World;
    public readonly List<Texture2D> SandTextures = new ();
    public Texture2D rocks;
    
    public MainGame()
    {
        Instance = this;
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        Window.AllowUserResizing = true;
        _audioManager = new AudioManager();
    }

    protected override void Initialize()
    {
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _renderTarget = new RenderTarget2D(GraphicsDevice, RendTargWidth, RendTargHeight);
        _font ??= Content.Load<BitmapFont>("ForwardFont");
        _audioManager.AddSfx("Error", "Error");
        _audioManager.AddSong("Warmer", "Warmer");
        _audioManager.PlaySong("Warmer");

        SandTextures.Add(Content.Load<Texture2D>("Sand1"));
        SandTextures.Add(Content.Load<Texture2D>("Sand2"));
        SandTextures.Add(Content.Load<Texture2D>("Sand3"));

        System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(UI).TypeHandle);
        System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(Resolution).TypeHandle);

        //Register Items
        ItemReference.RegisterItem("Wood", "Chunk of tree", Content.Load<Texture2D>("wood"));
        PlaceableItemReference.RegisterPlaceableItem(
            "Rocks", 
            "Just a few rocks", 
            typeof(TestRockEntity), 
            new ()
            {
                new Point(0, 0)
            }, 
            Content.Load<Texture2D>("rocks"));
        rocks = Content.Load<Texture2D>("rocks");
        
        var toolbar = new UIElement(new UI.Margin(180 - 18, 88, 0, 88), UI.Pixel, new Color(36, 36, 36));
        UI.Root.AddChild(toolbar);
        for (int i = 0; i < 8; i++)
        {
            var itemSlot = new ItemSlot(new Rectangle(0, 0, 16, 16), UI.Pixel, new Color(200, 200, 200));
            if (i == 0)
                itemSlot.Item = new Item("Wood", 4);
            if (i == 1)
                itemSlot.Item = new Item("Rocks", 150);
            toolbar.AddChild(itemSlot);
        }
        toolbar.ProcessUsingLayoutController(new HorizontalGrid(toolbar, new UI.Margin(1)));
        toolbar.ProcessUsingLayoutController(new FixedSize(16, 16));

        World = new World(WorldSizeX, WorldSizeY);
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
        GraphicsDevice.Clear(Color.DarkGoldenrod);

        OnDraw?.Invoke(deltaTime);
        
        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        
        OnDrawSprites?.Invoke(deltaTime, _spriteBatch);
        OnDrawUI?.Invoke(deltaTime, _spriteBatch);
        
        _spriteBatch.End();
        
        GraphicsDevice.SetRenderTarget(null);
        GraphicsDevice.Clear(Color.Black);
        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        _spriteBatch.Draw(_renderTarget, Resolution.TrimmedScreen, Color.White);
        _spriteBatch.End();
        
        base.Draw(gameTime);
    }
}