using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ReFactory.GameScripts.Machines.Testing;

public class TestRockEntity : GridEntity
{
    public override float DestroyTime => 0.1f;
    public override Texture2D Sprite => GameContent.Rocks;
    public override HashSet<Point> OccupiedRelativePoints => new() {new Point(0, 0)};
    public override Func<bool> OnDestroy => () => Inventory.AddItem(new Item("Rocks", 1));
}