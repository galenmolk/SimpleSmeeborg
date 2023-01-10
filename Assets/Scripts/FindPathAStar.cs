using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

namespace SimpleSmeeborg
{
    public class FindPathAStar : MonoBehaviour
    {
        public static Action<List<PathMarker>> OnPathComplete;

        public Maze Maze { get; private set; }
        public Material OpenMaterial;
        public Material ClosedMaterial;

        public List<PathMarker> open = new List<PathMarker>();
        public List<PathMarker> closed = new List<PathMarker>();

        public GameObject start;
        public GameObject end;
        public GameObject pathPoint;

        private PathMarker startNode;
        private PathMarker finishNode;

        private PathMarker lastPosition;

        private bool isPathComplete;

        public bool Run;

        private void OnValidate()
        {
            if (Run)
            {
                TryFindPath(lastPosition);
            }
        }

        private void BeginSearch()
        {
            isPathComplete = false;

            Cell startCell = Maze.StartCell;
            Cell finishCell = Maze.FinishCell;
            Vector2 startLocation = startCell.Coordinates;
            Vector2 finishLocation = finishCell.Coordinates;

            startNode = new PathMarker(startCell);
            finishNode = new PathMarker(finishCell);

            // possibly unnecessary for assessment.
            open.Clear();
            closed.Clear();

            open.Add(startNode);
            lastPosition = startNode;

            while (!TryFindPath(lastPosition))
            {

            }
        }

        private bool TryFindPath(PathMarker thisNode)
        {
            // Check to see if we've reached the finish.
            if (thisNode.Cell.CellType == CellType.FINISH)
            {
                isPathComplete = true;
    
                OnPathComplete?.Invoke(GetPath());
                return true;
            }

            List<Cell> neighbors = Maze.GetNeighbors(thisNode.Cell);

            foreach (Cell neighbor in Maze.GetNeighbors(thisNode.Cell))
            {
                Vector2Int neighborLocation = neighbor.Coordinates;
                if (IsClosed(neighborLocation))
                {
                    continue;
                }

                neighbor.instance.openClosedText.color = Color.red;

                float gCost = GetGCost(thisNode, neighborLocation);
                float hCost = GetHCost(neighborLocation);
                float fCost = gCost + hCost;

                if (!TryUpdateMarker(neighbor, gCost, hCost, fCost, thisNode))
                {
                    open.Add(new PathMarker(neighbor, gCost, hCost, fCost, thisNode));
                }
            }

            open = open.OrderBy(p => p.F).ToList<PathMarker>();
            PathMarker lowestCostPathMarker = open.ElementAt(0);
            closed.Add(lowestCostPathMarker);
            open.RemoveAt(0);

            lastPosition = lowestCostPathMarker;
            return false;
        }

        private bool IsClosed(Vector2Int location)
        {
            foreach (PathMarker pathMarker in closed)
            {
                if (pathMarker.Cell.Coordinates.Equals(location))
                {
                    return true;
                }
            }

            return false;
        }

        private bool TryUpdateMarker(Cell neighbor, float g, float h, float f, PathMarker parent)
        {
            foreach (PathMarker pathMarker in open)
            {
                if (pathMarker.Cell.Equals(neighbor))
                {
                    pathMarker.UpdateProperties(g, h, f, parent);
                    return true;
                }
            }

            return false;
        }

        private float GetGCost(PathMarker origin, Vector2Int neighborLocation)
        {
            float distanceToNeighbor = Vector2Int.Distance(origin.Cell.Coordinates, neighborLocation);
            return origin.G + distanceToNeighbor;
        }

        private float GetHCost(Vector2Int neighborLocation)
        {
            return Vector2Int.Distance(neighborLocation, finishNode.Cell.Coordinates);
        }

        private List<PathMarker> GetPath()
        {
            List<PathMarker> path = new List<PathMarker>();

            PathMarker thisPathMarker = lastPosition;

            while (thisPathMarker.Parent != null)
            {
                path.Insert(0, thisPathMarker);
                thisPathMarker = thisPathMarker.Parent;
            }

            return path;
        }

        private void Awake()
        {
            MazeLoader.OnMazeInitialized += HandleMazeInitialized;
        }

        private void OnDestroy()
        {
            MazeLoader.OnMazeInitialized -= HandleMazeInitialized;
        }

        private void HandleMazeInitialized(Maze maze)
        {
            Maze = maze;
            BeginSearch();
        }
    }
}
