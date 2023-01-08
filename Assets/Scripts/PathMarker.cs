using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleSmeeborg
{
    public class PathMarker
    {
        public Cell Cell;
        public float G;
        public float H;
        public float F;
        public Cell ParentCell;

        public PathMarker(Cell cell, float g = 0f, float h = 0f, float f = 0f, Cell parentCell = null)
        {
            Cell = cell;
            G = g;
            H = h;
            F = f;
            ParentCell = parentCell;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !GetType().Equals(obj.GetType()))
            {
                return false;
            }

            return (Cell.Equals(((Cell)obj)));
        }

        public override int GetHashCode()
        {
            return 0;
        }
    }
}
