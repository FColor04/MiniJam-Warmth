using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ReFactory.GameScripts;

namespace ReFactory.Utility;

public static class GridUtility
{
    public static IEnumerable<Point> GridNeighbours(this Point root)
    {
        yield return root + new Point(0, World.GridSize);
        yield return root + new Point(World.GridSize, 0);
        yield return root + new Point(0, -World.GridSize);
        yield return root + new Point(-World.GridSize, 0);
    }
}