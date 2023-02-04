using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MainGameFramework;
using NLua;
using ReFactory.UISystem;
using Debug = ReFactory.Debugger.Debug;

namespace ReFactory.GameScripts.Machines.ConveyorBelts
{

    public class ConveyorBelt : GridEntity
    {
        // private Lua luaBelt = new Lua();
        public static List<ConveyorBelt> ConveyorBelts = new();
        public override float DestroyTime => 0.1f; // Override GridEntity DestroyTime.
        private Func<Texture2D> _getSprite = () => GameContent.StraightConveyorBelt.GetSprite();

        public override HashSet<Point> OccupiedRelativePoints => new() { new Point(0, 0) }; // Override GridEntity OccupiedRelativePoints.
        public override Func<bool> OnDestroy => () => Inventory.AddItem(new Item("Belt", 1)); // Override GridEntity OnDestroy.
        public List<Entity> BeltEntities = new(); // List of entities on this Belt

        public ConveyorBelt()
        {
            ConveyorBelts.Add(this);
            MainGame.Instance.World.OnGridBuild += OnGridBuild;
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposing) return;
            ConveyorBelts.Remove(this);
            MainGame.Instance.World.OnGridBuild -= OnGridBuild;
            base.Dispose(true);
        }

        private void OnGridBuild(Point entityPosition, GridEntity entity)
        {
            if (entity is ConveyorBelt && Math.Abs(position.X - entityPosition.X) == World.GridSize || Math.Abs(position.Y - entityPosition.Y) == World.GridSize)
            {
                UpdateSprite();
            }
        }

        #region Global Update Method
        public static void GlobalUpdate(float deltaTime)
        {
            if(Keys.P.WasPressedThisFrame())
            { Debug.Log("new entity");
                ConveyorBelts.First().PlaceNewEntity(new BeltEntity()
                {
                    entity = new Entity()
                    {
                        internalSprite = UI.Pixel,
                        position = ConveyorBelts.First().position
                    }
                }, false);
                
            }
        }
        #endregion

        private void PlaceNewEntity(BeltEntity beltEntity, bool left)
        {
            beltEntity.pointA = position + -rotation.RotationToVector2() * World.GridSize;
            beltEntity.pointB = position;
        }

        #region Get Target Belt
        public ConveyorBelt GetTargetBelt()
        {
            Point beltTarget = position.ToPoint();
            switch (rotation)
            {
                case 0: //Top
                    beltTarget += new Point(0, -World.GridSize);
                    break;
                case 90: //Right
                    beltTarget += new Point(World.GridSize, 0);
                    break;
                case 180: //Bottom
                    beltTarget += new Point(0, World.GridSize);
                    break;
                case 270: //Left
                    beltTarget += new Point(-World.GridSize, 0);
                    break;
            }

            if (MainGame.Instance.World.gridElements.TryGetValue(beltTarget, out var entity) && entity is ConveyorBelt belt)
                return belt;
            return null;
        }

        #endregion

        #region Get Source Belt
        public ConveyorBelt GetSourceBelt()
        {
            Point beltSource = position.ToPoint();
            switch (rotation)
            {
                case 0: //Top
                    beltSource += new Point(0, World.GridSize);
                    break;
                case 90: //Right
                    beltSource += new Point(-World.GridSize, 0);
                    break;
                case 180: //Bottom
                    beltSource += new Point(0, -World.GridSize);
                    break;
                case 270: //Left
                    beltSource += new Point(World.GridSize, 0);
                    break;
            }

            if (MainGame.Instance.World.gridElements.TryGetValue(beltSource, out var entity) && entity is ConveyorBelt belt)
                return belt;
            return null;
        }

        #endregion

        #region Update Sprite Method
        public void UpdateSprite()
        {
            Point clockwisePoint = position.ToPoint();
            Point counterClockwisePoint = position.ToPoint();
            
            _getSprite = () => GameContent.StraightConveyorBelt.GetSprite();
        }
        #endregion

        public struct BeltEntity
        {
            enum BeltDirection
            {
                N_S, S_N, W_E, E_W, N_W, W_S, S_E, E_N, N_E, E_S, S_W, W_N
            }
            
            public Vector2 pointA;
            public Vector2 pointB;
            public Entity entity;
        }



    }
}