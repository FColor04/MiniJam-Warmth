using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MiniJam_Warmth.GameScripts.Machines;

public class TestRockEntity : GridEntity
{
    public override float DestroyTime => 0.1f;
    public override Texture2D sprite => GameContent.Rocks;
    public override HashSet<Point> OccupiedRelativePoints => new() {new Point(0, 0)};
    public override Func<bool> OnDestroy => () => Inventory.AddItem(new Item("Rocks", 1));
}