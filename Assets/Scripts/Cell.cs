namespace SimpleSmeeborg
{
    public class Cell
    {
        public CellType CellType { get; private set; }

        public int North { get; }
        public int South { get; }
        public int East { get; }
        public int West { get; }

        public int X { get; }
        public int Y { get; }

        public Cell(CellProperties cellProperties)
        {
            North = cellProperties.North;
            South = cellProperties.South;
            East = cellProperties.East;
            West = cellProperties.West;
            X = cellProperties.X;
            Y = cellProperties.Y;
        }

        public void SetType(CellType cellType)
        {
            CellType = cellType;
        }
    }
}
