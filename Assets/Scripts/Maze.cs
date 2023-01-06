namespace SimpleSmeeborg
{
    public class Maze
    {
        public Cell[][] CellMatrix { get; private set; }

        private AsciiMatrix asciiMatrix;

        public void InitializeMaze(string asciiInput)
        {
            asciiMatrix = new AsciiMatrix(asciiInput);
            CreateCellMatrix();
        }

        private void CreateCellMatrix()
        {
            int rowCount = GetCellIndexForAscii(asciiMatrix.AsciiRowCount, FormatConsts.CELL_Y_LENGTH);
            int columnCount = default;

            CellMatrix = new Cell[rowCount][];

            for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
            {
                columnCount = GetCellIndexForAscii(asciiMatrix.GetRowLength(rowIndex), FormatConsts.CELL_X_LENGTH);

                CellMatrix[rowIndex] = new Cell[columnCount];

                for (int columnIndex = 0; columnIndex < columnCount; columnIndex++)
                {
                    CellMatrix[rowIndex][columnIndex] = asciiMatrix.MakeCell(rowIndex, columnIndex);
                }
            }

            CellMatrix[0][0].SetType(CellType.START);
            CellMatrix[rowCount - 1][columnCount - 1].SetType(CellType.FINISH);
        }

        private int GetCellIndexForAscii(int asciiIndex, int dimensionDivisor)
        {
            return (asciiIndex - 1) / dimensionDivisor;
        }
    }
}
