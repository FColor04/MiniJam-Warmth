using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MiniJam_Warmth.GameScripts;

public abstract class GridEntity : Entity
{
    protected GridEntity() : base() {}
    public abstract HashSet<Point> OccupiedRelativePoints { get; }
}