using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleSmeeborg
{
    public class CellProperties
    {
        public int North { get; }
        public int South { get; }
        public int East { get; }
        public int West { get; }

        public int X { get; }
        public int Y { get; }

        public CellProperties(int north, int south, int east, int west, int x, int y)
        {
            North = north;
            South = south;
            East = east;
            West = west;
            X = x;
            Y = y;
        }
    }
}
