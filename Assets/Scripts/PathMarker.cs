using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleSmeeborg
{
    [Serializable]
    public class PathMarker
    {
        public Cell Cell { get; private set; }
        public float G { get; private set; }
        public float H { get; private set; }
        public float F { get; private set; }
        public PathMarker Parent { get; private set; }

        public PathMarker(Cell cell, float g = 0f, float h = 0f, float f = 0f, PathMarker parent = null)
        {
            Cell = cell;
            UpdateProperties(g, h, f, parent);
        }

        public void UpdateProperties(float g, float h, float f, PathMarker parent)
        {
            Cell.instance.gCost.text = $"G: {g.ToString()}"; 
            Cell.instance.hCost.text = $"H: {h.ToString()}"; 
            Cell.instance.fCost.text = $"F: {f.ToString()}"; 
            G = g;
            H = h;
            F = f;
            Parent = parent;
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
