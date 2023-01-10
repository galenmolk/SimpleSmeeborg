using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace SimpleSmeeborg
{
    public class Maze
    {
        public Cell StartCell { get; private set; }
        public Cell FinishCell { get; private set; }

        public Cell[][] CellArrays { get; private set; }

        public int Width { get; private set; }
        public int Height { get; private set; }

        private AsciiMaze asciiMatrix;

        private readonly List<Vector2Int> cardinalDirections = new List<Vector2Int>()
        {
            Vector2Int.up,
            Vector2Int.down,
            Vector2Int.right,
            Vector2Int.left
        };

        public void InitializeMaze(string asciiInput)
        {
            asciiMatrix = new AsciiMaze(asciiInput);
            CreateCellMatrix();
        }

        private void CreateCellMatrix()
        {
            Height = GetCellCountFromAscii(asciiMatrix.AsciiRowCount, FormatConsts.CELL_Y_LENGTH);

            CellArrays = new Cell[Height][];

            for (int rowIndex = 0; rowIndex < Height; rowIndex++)
            {
                Width = GetCellCountFromAscii(asciiMatrix.GetRowLength(rowIndex), FormatConsts.CELL_X_LENGTH);

                CellArrays[rowIndex] = new Cell[Width];

                for (int columnIndex = 0; columnIndex < Width; columnIndex++)
                {
                    CellArrays[rowIndex][columnIndex] = asciiMatrix.MakeCell(rowIndex, columnIndex);
                }
            }

            StartCell = CellArrays[0][0];
            FinishCell = CellArrays[Height - 1][Width - 1];

            StartCell.SetType(CellType.START);
            FinishCell.SetType(CellType.FINISH);
        }

        public List<Cell> GetNeighbors(Cell origin)
        {
            List<Cell> neighbors = new List<Cell>();

            Vector2Int cellLocation = origin.Coordinates;

            foreach (Vector2Int direction in cardinalDirections)
            {
                Vector2Int neighborLocation = cellLocation + direction;

                if (!TryGetCell(neighborLocation, out Cell neighbor))
                {
                    continue;
                }

                if (IsNeighborAccessible(origin, neighbor))
                {
                    neighbors.Add(neighbor);
                }
            }

            return neighbors;
        }

        private bool TryGetCell(Vector2Int position, out Cell cell)
        {
            cell = null;

            if (position.x < 0 || position.y < 0)
            {
                Debug.Log($"Cell position {position} must not be less than zero.");
                return false;
            }

            if (position.y > Height - 1)
            {
                Debug.Log($"Cell Y position {position} must not be greater than the number of rows {Height}.");
                return false;
            }

            int columnCount = CellArrays[position.y].Length;
            Debug.Log($"Column Count for row {position.x}: {columnCount}");

            if (position.x > columnCount - 1)
            {
                Debug.Log($"Cell X position {position} must not be greater than the number of columns {columnCount}.");
                return false;
            }

            cell = CellArrays[position.y][position.x];
            return true;
        }

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

            Debug.LogError($"Neighbor {neighbor.Coordinates} is not in one of the cardinal directions from origin {origin.Coordinates}.");
            return false;
        }

        private int GetCellCountFromAscii(int asciiIndex, int dimensionDivisor)
        {
            return (asciiIndex - 1) / dimensionDivisor;
        }
    }
}
