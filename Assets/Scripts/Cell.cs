using UnityEngine;

namespace SimpleSmeeborg
{
    public class Cell
    {
        public CellType CellType { get; private set; }

        public bool HasNorthPassage { get; }
        public bool HasSouthPassage { get; }
        public bool HasEastPassage { get; }
        public bool HasWestPassage { get; }

        public Vector2Int Coordinates { get; private set; }
        public Vector2 WorldPosition { get; private set; }

        public int X { get; }
        public int Y { get; }

        public Cell(bool north, bool south, bool east, bool west, int x, int y)
        {
            HasNorthPassage = north;
            HasSouthPassage = south;
            HasEastPassage = east;
            HasWestPassage = west;

            X = x;
            Y = y;
            Coordinates = new Vector2Int(x, y);

            // Invert the Y coordinate so 0,0 is the top-left of the maze.
            WorldPosition = new Vector2(x, -y);
        }

        public void SetType(CellType cellType)
        {
            CellType = cellType;
        }
    }
}
