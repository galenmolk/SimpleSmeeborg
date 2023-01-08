using UnityEngine;

namespace SimpleSmeeborg
{
    public class Cell
    {
        public CellType CellType { get; private set; }

        public bool HasNorthPassage => North == 0;
        public bool HasSouthPassage => South == 0;
        public bool HasEastPassage => East == 0;
        public bool HasWestPassage => West == 0;

        public int North { get; }
        public int South { get; }
        public int East { get; }
        public int West { get; }

        public int X { get; }
        public int Y { get; }

        public CellBehaviour instance;

        public Vector2Int Location { get; private set; }

        public Cell(CellProperties cellProperties)
        {
            North = cellProperties.North;
            South = cellProperties.South;
            East = cellProperties.East;
            West = cellProperties.West;
            X = cellProperties.X;
            Y = cellProperties.Y;
            Location = new Vector2Int(X, Y);
        }

        public bool IsAccessible()
        {
            return North == 0 || South == 0 || East == 0 || West == 0;
        }

        public void SetType(CellType cellType)
        {
            CellType = cellType;
        }

        public void SetMonoBehaviourInstance(CellBehaviour cellBehaviour)
        {
            instance = cellBehaviour;
        }

        public bool IsNorthOf(Cell cell)
        {
            return X == cell.X && Y == cell.Y - 1;
        }

        public bool IsSouthOf(Cell cell)
        {
            return X == cell.X && Y == cell.Y + 1;
        }

        public bool IsEastOf(Cell cell)
        {
            return X == cell.X + 1 && Y == cell.Y;
        }

        public bool IsWestOf(Cell cell)
        {
            return X == cell.X - 1 && Y == cell.Y;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !GetType().Equals(obj.GetType()))
            {
                return false;
            }

            Cell cell = obj as Cell;
            return (cell.X == X && cell.Y == Y);
        }

        public override int GetHashCode()
        {
            return 0;
        }
    }
}
