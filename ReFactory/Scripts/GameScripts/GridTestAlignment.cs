using MainGameFramework;
using Microsoft.Xna.Framework;
using ReFactory.GameScripts.Machines.ConveyorBelt;
using System.Diagnostics.CodeAnalysis;

public class GridTextAlignment
{
    private int gridSizeX;
    private int gridSizeY;

    public struct GridCell
    {
        private string cellName;
        private Vector2 position;
        private Vector2 rotation;
        private BeltParams_bkup.Belt[] belts;
        public override bool Equals([NotNullWhen(true)] object obj) {
            return base.Equals(obj); }
        public override int GetHashCode() {
            return base.GetHashCode(); }
        public override string ToString() {
            return base.ToString(); }

        public GridCell(Vector2 position, Vector2 rotation)
        {
            cellName = "Cell Position: " + position + "_Rotation: " + rotation;
            this.position = position;
            this.rotation = rotation;
            belts = new BeltParams_bkup.Belt[0];
        }
    }

    public void CreateGrid()
    {
        gridSizeX = 16;
        gridSizeY = 16;

        for (int i = 0; i < gridSizeX; i++){
            for(int j = 0; j < gridSizeY; j++)
            {
                
            }
        }
    }

}