using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ReFactory.GameScripts;

public class PlaceableItemReference : ItemReference
{
    public bool rotatable;
    public readonly Type placedEntityType;
    public HashSet<Point> occupiedPoints;
    public PlaceableItemReference(string name, string tooltip, Type placedEntityType, Texture2D sprite, int maxStack = 50, HashSet<Point> occupied = null, bool rotatable = false) : base(name, tooltip, sprite, maxStack)
    {
        this.placedEntityType = placedEntityType;
        if (occupied == null)
            occupied = new HashSet<Point>() {new Point(0, 0)};
        occupiedPoints = occupied;
        this.rotatable = rotatable;
    }

    public PlaceableItemReference(ItemReference reference, Type placedEntityType) : base(reference.name,
        reference.tooltip, reference.sprite, reference.maxStack)
    {
        this.placedEntityType = placedEntityType;
    }
    
    public static void RegisterPlaceableItem(string name, string tooltip, Type placedEntityType, HashSet<Point> occupiedPoints, Texture2D sprite, bool isRotatable = false, int maxStack = 50) =>
        ItemList.Items.Add(new PlaceableItemReference(name, tooltip, placedEntityType, sprite, maxStack, occupiedPoints, isRotatable));
}

public static class PlaceableItemExtensions
{
    public static PlaceableItemReference ToPlaceableItem(this ItemReference itemReference, Type placedEntityType) => new (itemReference, placedEntityType);
}