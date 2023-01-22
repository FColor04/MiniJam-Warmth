using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
    public Rectangle Bounds;
    public Rectangle InteractiveRect => new Rectangle(0, 0, 320, 180);
    public List<Entity> entities = new ();
    public Dictionary<Point, GridEntity> gridElements = new ();

    public Vector2 cameraOffset;
    private int[] _sandIndexes;
    
    private bool _drawPlaceable;
    private Texture2D _placeableTexture;
    private Rectangle _placeableRect;
    private Color _placeableColor;
    
    public World(int width, int height)
    {
        Bounds = new Rectangle(-width * GridSize / 2, -height * GridSize / 2, width * GridSize, height * GridSize);
        _sandIndexes = new int[width * height];

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

        cameraOffset.Y += Input.Vertical * deltaTime * GridSize * 5;
        cameraOffset.X += Input.Horizontal * deltaTime * GridSize * 5;
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