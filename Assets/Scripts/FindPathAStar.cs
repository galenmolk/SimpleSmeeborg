using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleSmeeborg
{
    public class FindPathAStar : MonoBehaviour
    {
        public Maze Maze { get; private set; }
        public Material OpenMaterial;
        public Material ClosedMaterial;

        List<PathMarker> open = new List<PathMarker>();
        List<PathMarker> closed = new List<PathMarker>();

        public GameObject start;
        public GameObject end;
        public GameObject pathPoint;

        private PathMarker startNode;
        private PathMarker finishNode;

        private PathMarker lastPosition;

        private bool isPathComplete;

        private void BeginSearch()
        {
            isPathComplete = false;

            Cell startCell = Maze.StartCell;
            Cell finishCell = Maze.FinishCell;
            Vector2 startLocation = startCell.Location;
            Vector2 finishLocation = finishCell.Location;

            Debug.Log("start: " + startLocation);
            Debug.Log("finish: " + finishLocation);

            startNode = new PathMarker(startCell);
            finishNode = new PathMarker(finishCell);

            // possibly unnecessary for assessment.
            open.Clear();
            closed.Clear();

            open.Add(startNode);
            lastPosition = startNode;
        }

        private void Search(PathMarker thisNode)
        {
            // Check to see if we've reached the finish.
            if (thisNode.Cell.CellType == CellType.FINISH)
            {
                isPathComplete = true;
                return;
            }

            foreach (Cell neighbor in Maze.GetNeighbors(thisNode.Cell))
            {
                Vector2Int neighborLocation = neighbor.Location;

                if (IsClosed(neighborLocation))
                {
                    continue;
                }

                float gCost = GetGCost(thisNode, neighborLocation);

                // next up H cost
            }
        }

        private bool IsClosed(Vector2Int location)
        {
            foreach (PathMarker pathMarker in closed)
            {
                if (pathMarker.Cell.Location.Equals(location))
                {
                    return true;
                }
            }

            return false;
        }

        private float GetGCost(PathMarker origin, Vector2Int neighborLocation)
        {
            float distanceToNeighbor = Vector2.Distance(origin.Cell.Location, neighborLocation);
            return origin.G + distanceToNeighbor;
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
