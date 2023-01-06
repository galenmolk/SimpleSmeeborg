using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleSmeeborg
{
    public class AsciiMatrix
    {
        private const int PASSABLE = 0;
        private const int IMPASSABLE = 1;
        private const char NEW_LINE = '\n';
        private const char BLANK_SPACE = ' ';

        private char[][] asciiMatrix;

        public int AsciiRowCount { get; private set; }
        private int[] asciiRowLengths;

        public AsciiMatrix(string asciiInput)
        {
            string[] rows = asciiInput.Split(NEW_LINE);

            AsciiRowCount = rows.Length;
            asciiRowLengths = new int[AsciiRowCount];

            // Assume that each row of characters contains the same length
            // -- i.e. a rectangular maze in the specified format.
            asciiMatrix = new char[AsciiRowCount][];

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
                GetNorthValue(asciiRow, asciiColumn),
                GetSouthValue(asciiRow, asciiColumn),
                GetEastValue(asciiRow, asciiColumn),
                GetWestValue(asciiRow, asciiColumn),
                cellRow, cellColumn);
        }

        private int GetAsciiIndexForCell(int index, int dimensionFactor)
        {
            return index * dimensionFactor;
        }

        private void CreateAsciiCharRow(string row, int rowIndex)
        {
            int rowLength = row.Length;
            asciiRowLengths[rowIndex] = rowLength;
            asciiMatrix[rowIndex] = new char[rowLength];

            for (int charIndex = 0; charIndex < rowLength; charIndex++)
            {
                asciiMatrix[rowIndex][charIndex] = row[charIndex];
            }
        }

        private int GetNorthValue(int row, int column)
        {
            char northWall = asciiMatrix[row][column + 1];
            return GetPassableValue(northWall);
        }

        private int GetSouthValue(int row, int column)
        {
            char southWall = asciiMatrix[row + FormatConsts.CELL_Y_LENGTH][column + 1];
            return GetPassableValue(southWall);
        }

        private int GetEastValue(int row, int column)
        {
            char eastWall = asciiMatrix[row + 1][column + FormatConsts.CELL_X_LENGTH];
            return GetPassableValue(eastWall);
        }

        private int GetWestValue(int row, int column)
        {
            char westWall = asciiMatrix[row + 1][column];
            return GetPassableValue(westWall);
        }

        private int GetPassableValue(char c)
        {
            return c.CompareTo(BLANK_SPACE) == 0 ? PASSABLE : IMPASSABLE;
        }
    }
}
