using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SystemDebugTools;
using System.Reflection.Metadata.Ecma335;
using AudioManagementUtil;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MiniJam_Warmth.GameScripts;

/// <summary>
/// A world is collection of entities, grid, collection of grid elements and player stats
/// it might also be called a level or scene.
/// </summary>
public class World : IPointerClickHandler
{
    private const int GridSize = 16;
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
    
    private bool _drawPlaceable;
    private Texture2D _placeableTexture;
    private Rectangle _placeableRect;
    private Color _placeableColor;
    
    public World(int width, int height)
    {
        Bounds = new Rectangle(BoundOriginX, BoundOriginY, (width * GridSize) - GridSize, (height * GridSize) - GridSize);
        _sandIndexes = new int[width * height];
        centerX = BoundOriginX + ((width * GridSize) - GridSize) / 2;
        centerY = BoundOriginY + ((height * GridSize) - GridSize) / 2;
        worldWidth = width;
        worldHeight = height;

        cameraOffset = new Vector2(-centerX / 4 , centerY / 4);


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
    }

    private void Draw(float deltaTime, SpriteBatch batch)
    {
        if (_drawPlaceable)
        {
            _drawPlaceable = false;
            _placeableRect.Location -= cameraOffset.ToPoint();
            batch.Draw(_placeableTexture, _placeableRect, _placeableColor);
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
            Debug.WriteLine($"Placed? {gridPosition}");

            gridEntity.position = gridPosition.ToVector2();
            foreach (var relativePoints in gridEntity.OccupiedRelativePoints)
            {
                gridElements.Add(gridPosition + new Point(relativePoints.X * GridSize, relativePoints.Y * GridSize), gridEntity);
            }
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
                    MainGame.AudioManager.PlaySfx("Error");
                
                if (PointerItemRenderer.HeldItem.Count == 0)
                    PointerItemRenderer.HeldItem = null;
            }
        }
    }

    private void DrawPlaceableGhost(PlaceableItemReference placeable)
    {
        _drawPlaceable = true;
        _placeableTexture = placeable.sprite;
        _placeableColor = CanBePlaced(placeable, GetGridPoint(Input.MousePositionWithinViewport.ToVector2() + cameraOffset)) ? new Color(0, 1, 0, 0.4f) : new Color(1, 0, 0, 0.2f);
        _placeableRect = new Rectangle(GetGridPoint(Input.MousePositionWithinViewport.ToVector2() + cameraOffset),
            new Point(32, 32));
    }
}