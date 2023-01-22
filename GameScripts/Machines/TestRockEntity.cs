using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MiniJam_Warmth.GameScripts.Machines;

public class TestRockEntity : GridEntity
{
    public override Texture2D sprite => MainGame.Instance.rocks;
    public override HashSet<Point> OccupiedRelativePoints => new() {new Point(0, 0)};
}