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
        public List<Entity> BeltEntities; // List of entities on this Belt

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

        #region Update Sprite Method
        public void UpdateSprite()
        {
            Point clockwisePoint = position.ToPoint();
            Point counterClockwisePoint = position.ToPoint();
            switch (rotation)
            {
                case 0: //Top
                    clockwisePoint += new Point(World.GridSize, 0);
                    counterClockwisePoint += new Point(-World.GridSize, 0);
                    break;
                case 90: //Right
                    clockwisePoint += new Point(0, World.GridSize);
                    counterClockwisePoint += new Point(0, -World.GridSize);
                    break;
                case 180: //Bottom
                    clockwisePoint += new Point(-World.GridSize, 0);
                    counterClockwisePoint += new Point(World.GridSize, 0);
                    break;
                case 270: //Left
                    clockwisePoint += new Point(0, -World.GridSize);
                    counterClockwisePoint += new Point(0, World.GridSize);
                    break;
            }

            bool clockwiseValid = MainGame.Instance.World.gridElements.TryGetValue(clockwisePoint, out var cwEntity)
                                && cwEntity is ConveyorBelt clockwiseBelt &&
                                clockwiseBelt.rotation == (rotation + 270) % 360;

            bool counterClockwiseValid = MainGame.Instance.World.gridElements.TryGetValue(counterClockwisePoint, out var ccwEntity)
                                && ccwEntity is ConveyorBelt counterClockwiseBelt &&
                                counterClockwiseBelt.rotation == (rotation + 90) % 360;
            if (!clockwiseValid && !counterClockwiseValid || clockwiseValid && counterClockwiseValid)
            {
                _getSprite = () => GameContent.StraightConveyorBelt.GetSprite();
                return;
            }

            _getSprite = clockwiseValid ? () => GameContent.ClockwiseConveyorBelt.GetSprite() : () => GameContent.CounterClockwiseConveyorBelt.GetSprite();
        }
        #endregion

        public struct BeltEntity
        {
            public Vector2 pointA;
            public Vector2 pointB;
            public Entity entity;
        }



    }
}