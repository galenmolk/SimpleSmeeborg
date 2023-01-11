using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SimpleSmeeborg
{
    /// <summary>
    /// FindPathAStar uses an implementation of the A* search algorithm 
    /// to find the shortest path of maze rooms from start to finish.
    /// </summary>
    public static class FindPathAStar
    {
        public static Action<List<PathNode>> OnPathComplete;
        public static Action OnPathFailed;

        private static Maze mazeInstance;

        private static List<PathNode> openNodes = new List<PathNode>();
        private static readonly List<PathNode> closedNodes = new List<PathNode>();

        private static PathNode startNode;
        private static PathNode finishNode;
        private static PathNode lastNode;

        private static bool isSearching;

        [RuntimeInitializeOnLoadMethod]
        private static void Initialize()
        {
            Application.quitting += HandleApplicationQuit;
            MazeLoader.OnMazeInitialized += HandleMazeInitialized;
        }

        private static void HandleApplicationQuit()
        {
            Application.quitting -= HandleApplicationQuit;
            MazeLoader.OnMazeInitialized -= HandleMazeInitialized;
        }

        private static void HandleMazeInitialized(Maze maze)
        {
            mazeInstance = maze;
            BeginSearch();
        }

        private static void BeginSearch()
        {
            startNode = new PathNode(mazeInstance.StartCell);
            finishNode = new PathNode(mazeInstance.FinishCell);

            openNodes.Add(startNode);
            lastNode = startNode;
            isSearching = true;

            SearchContinuously();
        }

        private static void SearchContinuously()
        {
            while (isSearching)
            {
                FindPath();
            }
        }

        private static void FindPath()
        {
            // Check to see if we've reached the finish node.
            if (IsPathComplete(lastNode))
            {
                isSearching = false;
                OnPathComplete?.Invoke(GetCompletePath());
                return;
            }

            SearchNeighbors(lastNode);
            ProcessCheapestNode();
        }

        private static bool IsPathComplete(PathNode currentNode)
        {
            return currentNode.CellType == CellType.FINISH;
        }

        private static List<PathNode> GetCompletePath()
        {
            List<PathNode> path = new List<PathNode>();
            PathNode thisNode = lastNode;

            while (thisNode.Parent != null)
            {
                path.Insert(0, thisNode);
                thisNode = thisNode.Parent;
            }

            path.Insert(0, startNode);
            return path;
        }

        private static void SearchNeighbors(PathNode currentNode)
        {
            List<Cell> neighbors = mazeInstance.GetValidNeighbors(currentNode.Cell);

            foreach (Cell neighbor in neighbors)
            {
                CalculateNeighborCosts(neighbor, currentNode);
            }
        }

        private static void CalculateNeighborCosts(Cell neighbor, PathNode currentNode)
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
            if (!TryUpdateNodeCosts(neighbor, gCost, fCost, currentNode))
            {
                openNodes.Add(new PathNode(neighbor, gCost, fCost, currentNode));
            }
        }

        private static float GetGCost(PathNode origin, Vector2Int neighborPos)
        {
            return origin.G + Vector2Int.Distance(origin.Coordinates, neighborPos);
        }

        private static float GetHCost(Vector2Int neighborLocation)
        {
            return Vector2Int.Distance(neighborLocation, finishNode.Coordinates);
        }

        private static bool IsNeighborInClosedList(Vector2Int neighborCoordinates)
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

        private static bool TryUpdateNodeCosts(Cell neighbor, float g, float f, PathNode parent)
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

        private static void ProcessCheapestNode()
        {
            if (openNodes.Count == 0)
            {
                Debug.LogError($"{nameof(FindPathAStar)}: no solution found.");
                isSearching = false;
                OnPathFailed?.Invoke();
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
    }
}
