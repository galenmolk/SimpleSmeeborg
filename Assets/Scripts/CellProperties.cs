using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleSmeeborg
{
    public class CellProperties
    {
        public bool HasNorthPassage { get; }
        public bool HasSouthPassage { get; }
        public bool HasEastPassage { get; }
        public bool HasWestPassage { get; }

        public int X { get; }
        public int Y { get; }

        public CellProperties(bool north, bool south, bool east, bool west, int x, int y)
        {
            HasNorthPassage = north;
            HasSouthPassage = south;
            HasEastPassage = east;
            HasWestPassage = west;
            X = x;
            Y = y;
        }
    }
}
