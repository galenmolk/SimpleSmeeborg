using System;
using UnityEngine;

namespace SimpleSmeeborg
{
    /// <summary>
    /// AsciiMaze generates a 2D char matrix from the inputted string.
    /// </summary>
    public class AsciiMaze
    {
        private const int CELL_X_LENGTH = 3;
        private const int CELL_Y_LENGTH = 2;

        private const char NEW_LINE = '\n';
        private const char BLANK_SPACE = ' ';

        public int Width { get; private set; }
        public int Height { get; private set; }

        private readonly char[,] charMatrix;

        public AsciiMaze(string asciiInput)
        {
            string[] lines = asciiInput.Split(NEW_LINE, StringSplitOptions.RemoveEmptyEntries);

            Height = lines.Length;

            if (Height == 0)
            {
                return;
            }

            // Assume that the ASCII maze has a uniform line length, clamping to the
            // shortest length if there is variation.
            Width = GetShortestLineLength(lines, Height);

            charMatrix = new char[Width, Height];

            for (int lineIndex = 0; lineIndex < Height; lineIndex++)
            {
                CreateAsciiCharArray(lines[lineIndex], lineIndex);
            }
        }

        public Cell MakeCell(int x, int y)
        {
            // Get the ASCII indices corresponding to the top-left corner of the cell.
            int asciiX = GetAsciiIndexForCell(x, CELL_X_LENGTH);
            int asciiY = GetAsciiIndexForCell(y, CELL_Y_LENGTH);

            // Create a new Cell with the appropriate accessibility.
            return new Cell(
                IsNorthPassable(asciiX, asciiY),
                IsSouthPassable(asciiX, asciiY),
                IsEastPassable(asciiX, asciiY),
                IsWestPassable(asciiX, asciiY),
                x, y);
        }

        public int GetCellXCount()
        {
            return (Width - 1) / CELL_X_LENGTH;
        }

        public int GetCellYCount()
        {
            return (Height - 1) / CELL_Y_LENGTH;
        }

        private int GetShortestLineLength(string[] lines, int lineCount)
        {
            int shortest = int.MaxValue;

            for (int i = 0; i < lineCount; i++)
            {
                int lineLength = lines[i].Length;
                shortest = Mathf.Min(shortest, lineLength);
            }

            return shortest;
        }

        private void CreateAsciiCharArray(string line, int lineIndex)
        {
            // Ignore chars if their index would exceed the maze width.
            int lineLength = Mathf.Min(line.Length, Width);

            for (int charIndex = 0; charIndex < lineLength; charIndex++)
            {
                charMatrix[charIndex, lineIndex] = line[charIndex];
            }
        }

        private int GetAsciiIndexForCell(int cellIndex, int dimensionFactor)
        {
            return cellIndex * dimensionFactor;
        }

        private bool IsNorthPassable(int x, int y)
        {
            // Get the leftmost char of the room's top wall.
            char northWall = charMatrix[x + 1, y];
            return IsCharPassable(northWall);
        }

        private bool IsSouthPassable(int x, int y)
        {
            // Get the leftmost char of the room's bottom wall.
            char southWall = charMatrix[x + 1, y + CELL_Y_LENGTH];
            return IsCharPassable(southWall);
        }

        private bool IsEastPassable(int x, int y)
        {
            // Get the char of the room's right wall.
            char eastWall = charMatrix[x + CELL_X_LENGTH, y + 1];
            return IsCharPassable(eastWall);
        }

        private bool IsWestPassable(int x, int y)
        {
            // Get the char of the room's left wall.
            char westWall = charMatrix[x, y + 1];
            return IsCharPassable(westWall);
        }

        private bool IsCharPassable(char c)
        {
            // In the given maze format, a blank space implies the absence of a wall.
            return c.CompareTo(BLANK_SPACE) == 0;
        }
    }
}
