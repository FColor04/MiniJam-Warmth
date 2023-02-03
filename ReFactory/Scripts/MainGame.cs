global using ReFactory.Debugger;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using AudioManagement;
using CanvasManagement;
using DefaultNamespace;
using ReFactory.Controllers;
using ReFactory.GameScripts;
using ReFactory.Utility;
using ReFactory;
using ReFactory.UISystem;
using ReFactory.UISystem.LayoutControllers;
using FontStashSharp;
using ParticleSystem.Particles;

namespace MainGameFramework {
    public class MainGame : Game
    {

        /// <summary>
        /// Called within <see cref="Update"/>>
        /// </summary>
        public static event Action<float> OnUpdate = _ => { };

        public static MainGame Instance;
        public static GraphicsDevice graphicsDevice => Instance.GraphicsDevice;
        private readonly GraphicsDeviceManager _graphics;
        public static GraphicsDeviceManager graphicsDeviceManager => Instance._graphics;
        public static ContentManager content => Instance.Content;
        public static bool IsFocused => Instance.IsActive;

        private StateMachine playerStateMachine;
        private int worldSizeX = 17;
        private int worldSizeY = 17;
        public World World;
        public int WorldSizeX { get { return worldSizeX; } }
        public int WorldSizeY { get { return worldSizeY; } }

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
            GameContent.RegisterItems();

            System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(CanvasManager).TypeHandle);
            System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(CameraController).TypeHandle);
            System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(AudioManager).TypeHandle);
            System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(UI).TypeHandle);
            System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(Resolution).TypeHandle);
            System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(ParticleTester).TypeHandle);

             var toolbar = new UIElement(new Rectangle(88, 180-20, 320-88-88, 20), UI.Pixel, ColorUtility.ToolbarGrey);
             UI.Root.AddChild(toolbar);
             for (int i = 0; i < 8; i++)
             {
                 var index = i;
                 var itemSlot = new ItemSlot(
                     () => Inventory.items[index],
                     item => Inventory.items[index] = item,
                     new Rectangle(0, 0, 16, 16),
                     UI.Pixel,
                     ColorUtility.ToolbarSilver);
                 toolbar.AddChild(itemSlot);
             }
            
             Inventory.AddItem(new Item("Belt", 24));
            
             toolbar.ProcessUsingLayoutController(new HorizontalGrid(toolbar));
             toolbar.ProcessUsingLayoutController(new FixedSize(16, 16));

            World = new World(WorldSizeX, WorldSizeY);
        }

        protected override void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Time.UnscaledDeltaTime = deltaTime;
            Time.UnscaledTotalTime = (float)gameTime.TotalGameTime.TotalSeconds;

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
            foreach (var canvas in CanvasManager.Canvases)
            {
                canvas.Draw();
            }
            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(Color.Black);
            foreach (var canvas in CanvasManager.Canvases)
            {
                canvas.DrawRenderTexture();
            }

            base.Draw(gameTime);
        }
    }
}