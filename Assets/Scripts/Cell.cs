using System;
using UnityEngine;

namespace SimpleSmeeborg
{
    [Serializable]
    public class Cell
    {
        public CellType CellType { get; private set; }

        public bool HasNorthPassage;
        public bool HasSouthPassage;
        public bool HasEastPassage ;
        public bool HasWestPassage;

        public int X;
        public int Y;

        public CellBehaviour instance;

        public Vector2Int Coordinates { get; private set; }
        public Vector2 WorldPosition { get; private set; }
        public Vector2 Position { get; private set; }

        public Cell(CellProperties cellProperties)
        {
            HasNorthPassage = cellProperties.HasNorthPassage;
            HasSouthPassage = cellProperties.HasSouthPassage;
            HasEastPassage = cellProperties.HasEastPassage;
            HasWestPassage = cellProperties.HasWestPassage;
            X = cellProperties.X;
            Y = cellProperties.Y;
            Coordinates = new Vector2Int(X, Y);
            Position = new Vector2(X, Y);
            WorldPosition = new Vector2(X, -Y);
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
