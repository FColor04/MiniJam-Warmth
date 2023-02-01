using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReFactory.GameScripts;
using MainGameFramework;
using ReFactory.GameScripts.Machines;
using ReFactory.GameScripts.Machines.ConveyorBelts;
using ReFactory.GameScripts.Machines.Testing;
using SpriteFontPlus;

namespace ReFactory
{
    public static class GameContent
    {
        public static readonly FontSystem FontSystem;
        public static readonly SpriteFontBase Font11;
        public static readonly ReadOnlyCollection<Texture2D> SandTextures;
        public static readonly Texture2D SelectedTile;
        public static readonly Texture2D Rocks;
        public static readonly SpriteSheetRenderer StraightConveyorBelt;
        public static readonly SpriteSheetRenderer ClockwiseConveyorBelt;
        public static readonly SpriteSheetRenderer CounterClockwiseConveyorBelt;
        
        static GameContent()
        {
            var Content = MainGame.content;
            
            //Load font
            FontSystem = new FontSystem();
            FontSystem.AddFont(Assembly.GetExecutingAssembly().GetManifestResourceStream("Fonts.FFFForward.ttf"));
            Font11 = FontSystem.GetFont(16);
            //
            SelectedTile = Content.Load<Texture2D>("Selected");
            Rocks = Content.Load<Texture2D>("rocks");
            
            var sandTextures = new List<Texture2D>()
            {
                Content.Load<Texture2D>("Sand1"),
                Content.Load<Texture2D>("Sand2"),
                Content.Load<Texture2D>("Sand3")
            };
            
            SandTextures = sandTextures.AsReadOnly();
            var conveyors = Content.Load<Texture2D>("Tiles/ConveyorBelts");
            StraightConveyorBelt = new SpriteSheetRenderer(conveyors, 
                4, 
                1, 
                SpriteSheetRenderer.Layer.Manual, 
                sourceRect: new Rectangle(0, 0, 16, 64));
            ClockwiseConveyorBelt = new SpriteSheetRenderer(conveyors, 
                4, 
                1, 
                SpriteSheetRenderer.Layer.Manual, 
                sourceRect: new Rectangle(80, 0, 16, 64));
            CounterClockwiseConveyorBelt = new SpriteSheetRenderer(conveyors, 
                4, 
                1, 
                SpriteSheetRenderer.Layer.Manual, 
                sourceRect: new Rectangle(64, 0, 16, 64));
        }

        public static void RegisterItems()
        {
            var Content = MainGame.content;

            ItemReference.RegisterItem("Wood", "Chunk of tree", Content.Load<Texture2D>("wood"));
            PlaceableItemReference.RegisterPlaceableItem(
                "Rocks", 
                "Just a few rocks", 
                typeof(TestRockEntity),
                new () {new Point(0, 0)}, 
                Content.Load<Texture2D>("rocks"));
            PlaceableItemReference.RegisterPlaceableItem(
                "Belt",
                "Classic conveyor belt",
                typeof(ConveyorBelt),
                new HashSet<Point>{new (0, 0)},
                Content.Load<Texture2D>("Tiles/ConveyorBelt"), 
                true);
        }
    }
}