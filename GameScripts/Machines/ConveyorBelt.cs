using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MiniJam_Warmth.Utility;

namespace MiniJam_Warmth.GameScripts.Machines;

public class ConveyorBelt : GridEntity
{
    public static List<ConveyorBelt> ConveyorBelts = new ();
    private static Stack<ConveyorBelt> _updateStack = new ();
    
    public override float DestroyTime => 0.1f;
    private Func<Texture2D> _getSprite = () => GameContent.StraightConveyorBelt.GetSprite();
    public override Texture2D sprite => _getSprite();
    public override HashSet<Point> OccupiedRelativePoints => new (){new Point(0, 0)};
    public override Func<bool> OnDestroy => () => Inventory.AddItem(new Item("Belt", 1));
    private bool _updatedThisFrame;
    private List<BeltEntity> _leftSideBeltEntities = new ();
    private List<BeltEntity> _rightSideBeltEntities = new ();

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

    public static void GlobalUpdate(float deltaTime)
    {
        if(Keys.P.WasPressedThisFrame() && ConveyorBelts.Any())
            ConveyorBelts.First().PlaceNewEntity(new BeltEntity()
            {
                entity = new Entity()
                {
                    internalSprite = UI.Pixel,
                    position = ConveyorBelts.First().position
                }
            }, false);

        RefreshUpdateStack();

        while (_updateStack.TryPop(out var belt))
        {
            var targetBelt = belt.GetTargetBelt();
            if (targetBelt != null)
            {
                float rotationDiff = targetBelt.rotation - belt.rotation;
                    
                while (rotationDiff < 0)
                    rotationDiff += 360;
                rotationDiff %= 360;
                bool targetOtherSide = (rotationDiff >= 180);

                if (belt._leftSideBeltEntities.Count == 0 && belt._rightSideBeltEntities.Count == 0) continue;
                
                if (targetBelt.HasSpaceForNewEntity(targetOtherSide))
                {
                    float previousProgress = 2;
                    foreach (var beltEntity in belt._rightSideBeltEntities.OrderByDescending(beltE => beltE.progress))
                    {
                        if (!beltEntity.Progress(deltaTime, ref previousProgress)) break;
                        belt._rightSideBeltEntities.Remove(beltEntity);
                        targetBelt.PlaceNewEntity(beltEntity, targetOtherSide);
                    }
                }else
                    Debug.WriteLine("No space for me A");

                if (targetBelt.HasSpaceForNewEntity(!targetOtherSide))
                {
                    var previousProgress = 1 + ItemSpacing;
                    foreach (var beltEntity in belt._leftSideBeltEntities.OrderByDescending(beltE => beltE.progress))
                    {
                        if (!beltEntity.Progress(deltaTime, ref previousProgress)) break;
                        belt._leftSideBeltEntities.Remove(beltEntity);
                        targetBelt.PlaceNewEntity(beltEntity, !targetOtherSide);
                    }
                }else
                    Debug.WriteLine("No space for me B");
            }else
                Debug.WriteLine($"No space for me C {belt.rotation} {belt.position}");
        }
    }

    private static void RefreshUpdateStack()
    {
        _updateStack.Clear();
        for (int i = 0; i < ConveyorBelts.Count; i++)
        {
            if (_updateStack.Contains(ConveyorBelts[i]))
                continue;
            
            _updateStack.Push(ConveyorBelts[i]);
            ConveyorBelt lastCheckedBelt = ConveyorBelts[i];
            ConveyorBelt sourceBelt = ConveyorBelts[i].GetSourceBelt();

            while(sourceBelt != null && sourceBelt != ConveyorBelts[i])
            {
                lastCheckedBelt = sourceBelt;
                sourceBelt = sourceBelt.GetSourceBelt();
            }

            while (lastCheckedBelt != null)
            {
                if (!_updateStack.Contains(lastCheckedBelt))
                {
                    _updateStack.Push(lastCheckedBelt);
                }
                else
                    break;
                lastCheckedBelt = lastCheckedBelt.GetTargetBelt();
            }
        }
    }

    private void PlaceNewEntity(BeltEntity beltEntity, bool left)
    {
        beltEntity.pointA = position + -rotation.RotationToVector2() * World.GridSize;
        beltEntity.pointB = position;
        
        if(left)
            _leftSideBeltEntities.Add(beltEntity);
        else
            _rightSideBeltEntities.Add(beltEntity);
    }

    private bool HasSpaceForNewEntity(bool left)
    {
        if (left)
        {
            return _leftSideBeltEntities.Count == 0 || _leftSideBeltEntities.MinBy(beltE => beltE.progress).progress > ItemSpacing;
        }
        return _rightSideBeltEntities.Count == 0 || _rightSideBeltEntities.MinBy(beltE => beltE.progress).progress > ItemSpacing;
    }
    private const float ItemSpacing = 1 / 4f;

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

        if (!(MainGame.Instance.World.gridElements.TryGetValue(beltTarget, out var entity) &&
              entity is ConveyorBelt belt))
            return null;
        return belt;
    }
    
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

        if (!(MainGame.Instance.World.gridElements.TryGetValue(beltSource, out var entity) &&
              entity is ConveyorBelt belt))
            return null;
        return belt;
    }

    private void OnGridBuild(Point entityPosition, GridEntity entity)
    {
        if (entity is ConveyorBelt belt && 
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            (Math.Abs(position.X - entityPosition.X) == World.GridSize) ||
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            (Math.Abs(position.Y - entityPosition.Y) == World.GridSize)
            )
        {
            UpdateSprite();
        }
    }

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
        if ((!clockwiseValid && !counterClockwiseValid) || (clockwiseValid && counterClockwiseValid))
        {
            _getSprite = () => GameContent.StraightConveyorBelt.GetSprite();
            return;
        }
        
        _getSprite = clockwiseValid ? () => GameContent.ClockwiseConveyorBelt.GetSprite() : () => GameContent.CounterClockwiseConveyorBelt.GetSprite();
    }

    public struct BeltEntity
    {
        public Vector2 pointA;
        public Vector2 pointB;
        public float progress;
        public Entity entity;

        public bool Progress(float deltaTime, ref float previousProgress)
        {
            if (progress + deltaTime >= previousProgress - ItemSpacing) return false;
            
            progress += deltaTime;
            entity.position = Vector2.Lerp(pointA, pointB, progress);
            previousProgress = progress;
            return progress >= 1;
        }
    }
}