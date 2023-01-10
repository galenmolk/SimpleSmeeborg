using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleSmeeborg
{
    public class AsciiMaze
    {
        private const int PASSABLE = 0;
        private const int IMPASSABLE = 1;
        private const char NEW_LINE = '\n';
        private const char BLANK_SPACE = ' ';

        private readonly char[][] asciiArrays;

        public int AsciiRowCount { get; private set; }
        private readonly int[] asciiRowLengths;

        public AsciiMaze(string asciiInput)
        {
            string[] rows = asciiInput.Split(NEW_LINE);

            AsciiRowCount = rows.Length;
            asciiRowLengths = new int[AsciiRowCount];

            asciiArrays = new char[AsciiRowCount][];

            for (int rowIndex = 0; rowIndex < AsciiRowCount; rowIndex++)
            {
                CreateAsciiCharRow(rows[rowIndex], rowIndex);
            }
        }

        public int GetRowLength(int rowIndex)
        {
            return asciiRowLengths[rowIndex];
        }

        public Cell MakeCell(int row, int column)
        {
            CellProperties properties = GetCellProps(row, column);
            return new Cell(properties);
        }

        public CellProperties GetCellProps(int cellRow, int cellColumn)
        {
            int asciiRow = GetAsciiIndexForCell(cellRow, FormatConsts.CELL_Y_LENGTH);
            int asciiColumn = GetAsciiIndexForCell(cellColumn, FormatConsts.CELL_X_LENGTH);

            return new CellProperties(
                IsNorthPassable(asciiRow, asciiColumn),
                IsSouthPassable(asciiRow, asciiColumn),
                IsEastPassable(asciiRow, asciiColumn),
                IsWestPassable(asciiRow, asciiColumn),
                cellColumn, cellRow);
        }

        private int GetAsciiIndexForCell(int index, int dimensionFactor)
        {
            return index * dimensionFactor;
        }

        private void CreateAsciiCharRow(string row, int rowIndex)
        {
            int rowLength = row.Length;
            asciiRowLengths[rowIndex] = rowLength;
            asciiArrays[rowIndex] = new char[rowLength];

            for (int charIndex = 0; charIndex < rowLength; charIndex++)
            {
                asciiArrays[rowIndex][charIndex] = row[charIndex];
            }
        }

        private bool IsNorthPassable(int row, int column)
        {
            char northWall = asciiArrays[row][column + 1];
            return IsPassable(northWall);
        }

        private bool IsSouthPassable(int row, int column)
        {
            char southWall = asciiArrays[row + FormatConsts.CELL_Y_LENGTH][column + 1];
            return IsPassable(southWall);
        }

        private bool IsEastPassable(int row, int column)
        {
            char eastWall = asciiArrays[row + 1][column + FormatConsts.CELL_X_LENGTH];
            return IsPassable(eastWall);
        }

        private bool IsWestPassable(int row, int column)
        {
            char westWall = asciiArrays[row + 1][column];
            return IsPassable(westWall);
        }

        private bool IsPassable(char c)
        {
            return c.CompareTo(BLANK_SPACE) == 0;
        }
    }
}
