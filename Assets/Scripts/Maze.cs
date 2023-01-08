using System.Collections.Generic;
using UnityEngine;

namespace SimpleSmeeborg
{
    public class Maze
    {
        public Cell StartCell { get; private set; }
        public Cell FinishCell { get; private set; }

        public Cell[][] CellArrays { get; private set; }

        private AsciiMatrix asciiMatrix;

        private int rowCount;

        private readonly List<Vector2Int> cardinalDirections = new List<Vector2Int>()
        {
            Vector2Int.up,
            Vector2Int.down,
            Vector2Int.right,
            Vector2Int.left
        };

        public void InitializeMaze(string asciiInput)
        {
            asciiMatrix = new AsciiMatrix(asciiInput);
            CreateCellMatrix();
        }

        private void CreateCellMatrix()
        {
            rowCount = GetCellIndexForAscii(asciiMatrix.AsciiRowCount, FormatConsts.CELL_Y_LENGTH);
            int columnCount = default;

            CellArrays = new Cell[rowCount][];

            for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
            {
                columnCount = GetCellIndexForAscii(asciiMatrix.GetRowLength(rowIndex), FormatConsts.CELL_X_LENGTH);

                CellArrays[rowIndex] = new Cell[columnCount];

                for (int columnIndex = 0; columnIndex < columnCount; columnIndex++)
                {
                    CellArrays[rowIndex][columnIndex] = asciiMatrix.MakeCell(rowIndex, columnIndex);
                }
            }

            StartCell = CellArrays[0][0];
            FinishCell = CellArrays[rowCount - 1][columnCount - 1];

            StartCell.SetType(CellType.START);
            FinishCell.SetType(CellType.FINISH);
        }

        public List<Cell> GetNeighbors(Cell origin)
        {
            List<Cell> neighbors = new List<Cell>();

            Vector2Int cellLocation = origin.Location;

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

            if (position.x < 0f || position.y < 0f)
            {
                Debug.LogError("Cell position must not be less than zero.");
                return false;
            }

            if (position.y > rowCount - 1)
            {
                Debug.LogError($"Cell Y position must not be greater than the number of rows {rowCount}.");
                return false;
            }


            int columnCount = CellArrays[position.x].Length;

            if (position.x > columnCount - 1)
            {
                Debug.LogError($"Cell X position must not be greater than the number of columns {columnCount}.");
                return false;
            }

            cell = CellArrays[position.x][position.y];
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

            Debug.LogError("Neighbor is not in one of the cardinal directions from origin.");
            return false;
        }

        private int GetCellIndexForAscii(int asciiIndex, int dimensionDivisor)
        {
            return (asciiIndex - 1) / dimensionDivisor;
        }
    }
}
