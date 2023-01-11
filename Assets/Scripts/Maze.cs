using System.Collections.Generic;
using UnityEngine;

namespace SimpleSmeeborg
{
    public class Maze
    {
        public Cell StartCell { get; private set; }
        public Cell FinishCell { get; private set; }

        public int Width { get; private set; }
        public int Height { get; private set; }

        private readonly AsciiMaze asciiMaze;
        private Cell[,] cellMatrix;

        private readonly List<Vector2Int> cardinalDirections = new List<Vector2Int>()
        {
            Vector2Int.up,
            Vector2Int.down,
            Vector2Int.right,
            Vector2Int.left
        };

        public Maze(string asciiInput)
        {
            asciiMaze = new AsciiMaze(asciiInput);
            CreateCellMatrix();
        }

        private void CreateCellMatrix()
        {
            Width = asciiMaze.GetCellXCount();
            Height = asciiMaze.GetCellYCount();

            cellMatrix = new Cell[Width, Height];

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    cellMatrix[x, y] = asciiMaze.MakeCell(x, y);
                }
            }

            StartCell = cellMatrix[0, 0];
            FinishCell = cellMatrix[Width - 1, Height - 1];

            StartCell.SetType(CellType.START);
            FinishCell.SetType(CellType.FINISH);
        }

        public Cell GetCell(int x, int y)
        {
            return cellMatrix[x, y];
        }

        public List<Cell> GetValidNeighbors(Cell origin)
        {
            List<Cell> neighbors = new List<Cell>();

            foreach (Vector2Int direction in cardinalDirections)
            {
                // Offset the origin coordinates by each of the cardinal directions
                // to find its neighbors.
                Vector2Int coordinates = origin.Coordinates + direction;

                // Check to see if this coordinate exists in the maze.
                if (!TryGetCell(coordinates, out Cell neighbor))
                {
                    continue;
                }

                // Consider the neighbor valid if it is unblocked by a wall.
                if (IsNeighborAccessible(origin, neighbor))
                {
                    neighbors.Add(neighbor);
                }
            }

            return neighbors;
        }

        private bool TryGetCell(Vector2Int coordinates, out Cell cell)
        {
            cell = null;
            int x = coordinates.x;
            int y = coordinates.y;

            // Verify that the coordinates are within the bounds of the maze.
            if (IsPositionInMaze(x, y))
            {
                cell = cellMatrix[x, y];
                return true;
            }

            return false;
        }

        private bool IsPositionInMaze(int x, int y)
        {
            return x >= 0 && x < Width && y >= 0 && y < Height;
        }

        /// <summary>
        /// IsNeighborAccessible finds the directional relationship between two
        /// cells. It returns true if no walls exist between the two cells.
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="neighbor"></param>
        /// <returns></returns>
        private bool IsNeighborAccessible(Cell origin, Cell neighbor)
        {
            if (origin.IsNorthOf(neighbor))
            {
                return origin.HasSouthPassage && neighbor.HasNorthPassage;
            }
            else if (origin.IsSouthOf(neighbor))
            {
                return origin.HasNorthPassage && neighbor.HasSouthPassage;
            }
            else if (origin.IsEastOf(neighbor))
            {
                return origin.HasWestPassage && neighbor.HasEastPassage;
            }
            else if (origin.IsWestOf(neighbor))
            {
                return origin.HasEastPassage && neighbor.HasWestPassage;
            }

            Debug.LogError($"{nameof(Maze)}.{nameof(IsNeighborAccessible)}: " +
                $"Neighbor ({neighbor.Coordinates}) is not in one of the cardinal " +
                $"directions from the origin ({origin.Coordinates}).");

            return false;
        }
    }
}
