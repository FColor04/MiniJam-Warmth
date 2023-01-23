using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SystemDebugTools;
using System.Reflection.Metadata.Ecma335;
using AudioManagementUtil;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MiniJam_Warmth.GameScripts.Machines;
using MiniJam_Warmth.Utility;

namespace MiniJam_Warmth.GameScripts;

/// <summary>
/// A world is collection of entities, grid, collection of grid elements and player stats
/// it might also be called a level or scene.
/// </summary>
public class World : IPointerClickHandler
{
    public const int GridSize = 16;

    public event Action<Point, GridEntity> OnGridBuild = (_, _) => {};
    
    private int interactiveRectA = 0;
    private int interactiveRectB = 0;
    private int interactiveRectC = 320;
    private int interactiveRectD = 180;
    
    public Rectangle Bounds;
    private int BoundOriginX = 0;
    private int BoundOriginY = 0;
    private float centerX;
    private float centerY;
    private int worldWidth;
    private int worldHeight;
    public Rectangle InteractiveRect => new Rectangle(interactiveRectA, interactiveRectB, interactiveRectC, interactiveRectD);
    public List<Entity> entities = new();
    public Dictionary<Point, GridEntity> gridElements = new ();

    public Vector2 cameraOffset;
    private int[] _sandIndexes;

    private float _destroyProgress;
    private ProgressBar _destroyFillBar;
    
    private bool _drawPlaceable;
    private Texture2D _placeableTexture;
    private Rectangle _placeableRect;
    private Color _placeableColor;
    private bool _isPlaceableRotatable;
    private float _placeableRotation;

    private float _desertStormTime;
    private SpriteSheetRenderer _desertStorm;
    
    public World(int width, int height)
    {
        Bounds = new Rectangle(BoundOriginX, BoundOriginY, (width * GridSize) - GridSize, (height * GridSize) - GridSize);
        _sandIndexes = new int[width * height];
        centerX = BoundOriginX + ((width * GridSize) - GridSize) / 2;
        centerY = BoundOriginY + ((height * GridSize) - GridSize) / 2;
        worldWidth = width;
        worldHeight = height;

        cameraOffset = new Vector2(-centerX / 4 , centerY / 4);

        
        _desertStorm = new SpriteSheetRenderer(MainGame.content.Load<Texture2D>("SandStorm/SandStorm"), 7, 7, SpriteSheetRenderer.Layer.UI);
        _desertStormTime = Random.Shared.Range(20, 70);
        
        _desertStorm.SetColor(new Color(0,0,0,0));
        _desertStorm.SetRect(new Rectangle(0, 0, 320, 180));
        
        for (int x = Bounds.X; x < Bounds.Width; x += 32)
        {
            for (int y = Bounds.Y; y < Bounds.Height; y += 32)
            {
                entities.Add(new Sand(new Vector2(x, y)));
            }
        }
        
        MainGame.OnDrawSprites += Draw;
        MainGame.OnUpdate += Update;
        UI.AdditionalInteractiveElements.Add(this);
    }

    ~World()
    {
        MainGame.OnDrawSprites -= Draw;
        MainGame.OnUpdate -= Update;
        UI.AdditionalInteractiveElements.Remove(this);
    }

    private void Update(float deltaTime)
    {
        if (Time.TotalTime > _desertStormTime)
        {
            _desertStormTime += Random.Shared.Range(20, 70);
            _desertStorm.SetColor(_desertStorm.GetColor() == Color.White ? new Color(0,0,0,0) : Color.White);
        }
        
        if (PointerItemRenderer.HeldItem != null && PointerItemRenderer.HeldItem.Reference is PlaceableItemReference reference)
        {
            DrawPlaceableGhost(reference);
        }

        cameraOffset.Y += Input.Vertical * deltaTime * GridSize * 8;
        cameraOffset.X += Input.Horizontal * deltaTime * GridSize * 8;

        if(cameraOffset.X >= ((worldWidth * GridSize) - GridSize) / 2 || cameraOffset.X <= -((worldWidth * GridSize) - GridSize) / 2)
        {
            cameraOffset.X -= Input.Horizontal * deltaTime * GridSize * 8;
        }
        if (cameraOffset.Y >= (((worldWidth * GridSize) - GridSize) * 3) / 4 || cameraOffset.Y <= -((worldWidth * GridSize) - GridSize) / 4)
        {
            cameraOffset.Y -= Input.Vertical * deltaTime * GridSize * 8;
        }

        var mousePos = GetMouseGridPosition();
        if (Input.RightMouseHold && gridElements.ContainsKey(mousePos))
        {
            var gridEntityUnderMouse = gridElements[mousePos];

            _destroyProgress += deltaTime;

            if (_destroyProgress >= gridEntityUnderMouse.DestroyTime && (gridEntityUnderMouse.OnDestroy?.Invoke() ?? true))
            {
                var references = gridElements.Where(pair => pair.Value == gridEntityUnderMouse)
                    .Select(pair => pair.Key);

                foreach (var referencePoint in references)
                    gridElements.Remove(referencePoint);

                gridEntityUnderMouse.Dispose();
            }

            if (_destroyFillBar == null)
            {
                _destroyFillBar = new ProgressBar(
                    new Rectangle(88, 180 - 24, 320 - 88 - 88, 6), 
                    ColorUtility.ToolbarGrey, 
                    ColorUtility.FillBarYellow,
                    UI.Pixel);
                UI.Root.AddChild(_destroyFillBar);
            }
            _destroyFillBar.fillAmount = _destroyProgress / gridEntityUnderMouse.DestroyTime;
        }
        else
        {
            _destroyProgress = 0;
            _destroyFillBar?.Dispose();
            _destroyFillBar = null;
        }
        
        ConveyorBelt.GlobalUpdate(deltaTime);
    }

    private void Draw(float deltaTime, SpriteBatch batch)
    {
        if (_drawPlaceable)
        {
            _drawPlaceable = false;
            _placeableRect.Location -= cameraOffset.ToPoint();
            batch.Draw(_placeableTexture, _placeableRect, null, _placeableColor, _isPlaceableRotatable ? MathHelper.ToRadians(_placeableRotation) : 0, new Vector2(_placeableRect.Width / 2f, _placeableRect.Height /2f), SpriteEffects.None, 0);
        } else
        {
            var pos = GetGridPoint(Input.MousePositionWithinViewport.ToVector2() + cameraOffset) - cameraOffset.ToPoint();
            batch.Draw(GameContent.SelectedTile, new Rectangle(pos, new Point(16, 16)), Color.White);
        }
    }

    public bool IsPointOccupied(Point gridPoint) => gridElements.ContainsKey(gridPoint) || !Bounds.Contains(gridPoint);

    public bool CanBePlaced(PlaceableItemReference item, Point root) => item.OccupiedPoints.All(point => !IsPointOccupied(root + new Point(point.X * GridSize, point.Y * GridSize)));
    
    public bool PlaceItem(Vector2 position, PlaceableItemReference item)
    {
        var gridPosition = GetGridPoint(position);
        if (!CanBePlaced(item, gridPosition)) return false;
        
        var entityInstance = Activator.CreateInstance(item.PlacedEntityType);
        if (entityInstance is GridEntity gridEntity)
        {
            MainGame.AudioManager.PlaySfx(AudioManager.Sfx.Place, 0.5f, Random.Shared.Range(-0.2f, 0.2f));

            if (item.rotatable)
                gridEntity.rotation = _placeableRotation;
            
            gridEntity.position = gridPosition.ToVector2();
            foreach (var relativePoints in gridEntity.OccupiedRelativePoints)
            {
                var pointGridPosition =
                    gridPosition + new Point(relativePoints.X * GridSize, relativePoints.Y * GridSize);
                gridElements.Add(pointGridPosition, gridEntity);
                OnGridBuild?.Invoke(pointGridPosition, gridEntity);
            }

            if (gridEntity is ConveyorBelt belt)
                belt.UpdateSprite();

        }else if (entityInstance is Entity entity)
        {
            entity.position = position;
            entities.Add(entity);
            Debug.WriteLine("Placed Entity???");
        }
        
        return true;
    }

    private Point GetGridPoint(Vector2 position)
    {
        var x = ((int) Math.Floor(position.X / GridSize)) * GridSize;
        var y = ((int) Math.Floor(position.Y / GridSize)) * GridSize;
        return new Point(x, y);
    }

    public void OnPointerClick(int buttonIndex)
    {
        if (buttonIndex == 0)
        {
            //Handle building first
            if (PointerItemRenderer.HeldItem != null && PointerItemRenderer.HeldItem.Reference is PlaceableItemReference reference)
            {
                if(PlaceItem(Input.MousePositionWithinViewport.ToVector2() + cameraOffset, reference))
                    PointerItemRenderer.HeldItem.Count--;
                else
                    MainGame.AudioManager.PlaySfx(AudioManager.Sfx.Error);
            }
        }
    }

    private void DrawPlaceableGhost(PlaceableItemReference placeable)
    {
        _drawPlaceable = true;
        _placeableTexture = placeable.sprite;
        _placeableColor = CanBePlaced(placeable, GetMouseGridPosition()) ? new Color(0, 1, 0, 0.4f) : new Color(1, 0, 0, 0.2f);
        var size = new Point(_placeableTexture.Width, _placeableTexture.Height);
        _placeableRect = new Rectangle(GetMouseGridPosition() + new Point(size.X / 2, size.Y / 2), size);
        _isPlaceableRotatable = placeable.rotatable;
        
        if (Keys.LeftShift.IsHeldThisFrame() && Keys.R.WasPressedThisFrame() && placeable.rotatable)
            _placeableRotation -= 90;
        else if (Keys.R.WasPressedThisFrame() && placeable.rotatable)
            _placeableRotation += 90;

        while (_placeableRotation < 0)
            _placeableRotation += 360;
        
        _placeableRotation %= 360;
    }

    public Point GetMouseGridPosition()
    {
        return GetGridPoint(Input.MousePositionWithinViewport.ToVector2() + cameraOffset);
    }
}