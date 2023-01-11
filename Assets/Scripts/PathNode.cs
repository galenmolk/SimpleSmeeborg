using UnityEngine;

namespace SimpleSmeeborg
{
    public class PathNode
    {
        public Cell Cell { get; private set; }
        public PathNode Parent { get; private set; }

        public float G { get; private set; }
        public float F { get; private set; }

        // Convenience properties for classes utilizing PathNode.
        public Vector2Int Coordinates => Cell.Coordinates;
        public Vector2 WorldPosition => Cell.WorldPosition;
        public CellType CellType => Cell.CellType;

        public PathNode(Cell cell, float g = 0f, float f = 0f, PathNode parent = null)
        {
            Cell = cell;
            UpdateProperties(g, f, parent);
        }

        public void UpdateProperties(float g, float f, PathNode parent)
        {
            G = g;
            F = f;
            Parent = parent;
        }
    }
}
