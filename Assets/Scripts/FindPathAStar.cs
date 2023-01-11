using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SimpleSmeeborg
{
    /// <summary>
    /// FindPathAStar uses an implementation of the A Star search algorithm 
    /// to find the shortest path of maze cells from start to finish.
    /// </summary>
    public class FindPathAStar : MonoBehaviour
    {
        public static Action<List<PathNode>> OnPathComplete;

        private Maze maze;

        private List<PathNode> openNodes = new List<PathNode>();
        private readonly List<PathNode> closedNodes = new List<PathNode>();

        private PathNode startNode;
        private PathNode finishNode;
        private PathNode lastNode;

        private bool isSearching;

        private void BeginSearch()
        {
            startNode = new PathNode(maze.StartCell);
            finishNode = new PathNode(maze.FinishCell);

            openNodes.Add(startNode);
            lastNode = startNode;
            isSearching = true;

            StartCoroutine(SearchContinuously());
        }

        private IEnumerator SearchContinuously()
        {
            while (isSearching)
            {
                FindPath();
                yield return null;
            }
        }

        private void FindPath()
        {
            // Check to see if we've reached the finish node.
            if (IsPathComplete(lastNode))
            {
                OnPathComplete?.Invoke(GetCompletePath());
                isSearching = false;
            }

            SearchNeighbors(lastNode);
            ProcessCheapestNode();
        }

        private bool IsPathComplete(PathNode currentNode)
        {
            return currentNode.CellType == CellType.FINISH;
        }

        private List<PathNode> GetCompletePath()
        {
            List<PathNode> path = new List<PathNode>();
            PathNode thisNode = lastNode;

            while (thisNode.Parent != null && thisNode.CellType != CellType.START)
            {
                path.Insert(0, thisNode);
                thisNode = thisNode.Parent;
            }

            return path;
        }

        private void SearchNeighbors(PathNode currentNode)
        {
            List<Cell> neighbors = maze.GetValidNeighbors(currentNode.Cell);

            foreach (Cell neighbor in neighbors)
            {
                CalculateNeighborCosts(neighbor, currentNode);
            }
        }

        private void CalculateNeighborCosts(Cell neighbor, PathNode currentNode)
        {
            Vector2Int neighborLocation = neighbor.Coordinates;

            // Ignore neighbors in the closed list.
            if (IsNeighborInClosedList(neighborLocation))
            {
                return;
            }

            // Get distance from current node to neighbor.
            float gCost = GetGCost(currentNode, neighborLocation);

            // Get distance from neighbor to goal node.
            float hCost = GetHCost(neighborLocation);

            // Get combined cost for neighbor node.
            float fCost = gCost + hCost;

            // Update the node costs if one exists -- otherwise, add a new open node.
            if (!TryUpdateNodeCosts(neighbor, gCost, hCost, fCost, currentNode))
            {
                openNodes.Add(new PathNode(neighbor, gCost, fCost, currentNode));
            }
        }

        private float GetGCost(PathNode origin, Vector2Int neighborPos)
        {
            return origin.G + Vector2Int.Distance(origin.Coordinates, neighborPos);
        }

        private float GetHCost(Vector2Int neighborLocation)
        {
            return Vector2Int.Distance(neighborLocation, finishNode.Coordinates);
        }

        private bool IsNeighborInClosedList(Vector2Int neighborCoordinates)
        {
            for (int i = 0, count = closedNodes.Count; i < count; i++)
            {
                if (closedNodes[i].Coordinates.Equals(neighborCoordinates))
                {
                    return true;
                }
            }

            return false;
        }

        private bool TryUpdateNodeCosts(Cell neighbor, float g, float h, float f, PathNode parent)
        {
            foreach (PathNode node in openNodes)
            {
                if (node.Coordinates.Equals(neighbor.Coordinates))
                {
                    node.UpdateProperties(g, f, parent);
                    return true;
                }
            }

            return false;
        }

        private void ProcessCheapestNode()
        {
            if (openNodes.Count == 0)
            {
                Debug.LogError($"{nameof(FindPathAStar)}: no solution found.");
                isSearching = false;
                return;
            }

            // Use LINQ to order the open nodes by ascending cost.
            openNodes = openNodes.OrderBy(p => p.F).ToList<PathNode>();

            PathNode cheapestNode = openNodes.ElementAt(0);

            // Move the cheapest node from the open list to the closed list.
            closedNodes.Add(cheapestNode);
            openNodes.RemoveAt(0);

            lastNode = cheapestNode;
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
            this.maze = maze;
            BeginSearch();
        }
    }
}
