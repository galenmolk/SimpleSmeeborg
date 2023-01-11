using System;
using UnityEngine;

namespace SimpleSmeeborg
{
    public class MazeLoader : MonoBehaviour
    {
        public static Action<Maze> OnMazeInitialized;

        [SerializeField] private TextAsset inputAscii;
        [SerializeField] private Transform cellParent;
        [SerializeField] private CellBehaviour cellPrefab;

        private void Start()
        {
            if (InputExists())
            {
                LoadMaze();
            }
            else
            {
                Debug.LogError($"{nameof(MazeLoader)}: No input found.");
            }
        }

        private bool InputExists()
        {
            return inputAscii != null && !string.IsNullOrWhiteSpace(inputAscii.text);
        }

        private void LoadMaze()
        {
            Maze maze = new Maze(inputAscii.text);

            for (int x = 0; x < maze.Width; x++)
            {
                for (int y = 0; y < maze.Height; y++)
                {
                    CreateCellBehaviour(maze.GetCell(x, y));
                }
            }

            OnMazeInitialized?.Invoke(maze);
        }

        private void CreateCellBehaviour(Cell cell)
        {
            CellBehaviour cellBehaviour = Instantiate(
                cellPrefab,
                cell.WorldPosition,
                Quaternion.identity,
                cellParent);

            cellBehaviour.InitializeCell(cell);
        }
    }
}
