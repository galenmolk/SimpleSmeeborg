namespace SimpleSmeeborg
{
    public static class CellExtensions
    {
        public static bool IsNorthOf(this Cell cell, Cell otherCell)
        {
            return cell.X == otherCell.X && cell.Y == otherCell.Y - 1;
        }

        public static bool IsSouthOf(this Cell cell, Cell otherCell)
        {
            return cell.X == otherCell.X && cell.Y == otherCell.Y + 1;
        }

        public static bool IsEastOf(this Cell cell, Cell otherCell)
        {
            return cell.X == otherCell.X + 1 && cell.Y == otherCell.Y;
        }

        public static bool IsWestOf(this Cell cell, Cell otherCell)
        {
            return cell.X == otherCell.X - 1 && cell.Y == otherCell.Y;
        }
    }
}
