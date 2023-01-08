using System;
using UnityEngine;

namespace SimpleSmeeborg
{
    public class MazeLoader : MonoBehaviour
    {
        public static Action<Maze> OnMazeInitialized;

        [SerializeField] private TextAsset inputAscii;
        [SerializeField] private RectTransform cellParent;
        [SerializeField] private CellBehaviour cellPrefab;

        private string asciiString;

        private void Start()
        {
            asciiString = inputAscii.text;

            Maze maze = new Maze();

            maze.InitializeMaze(asciiString);

            for (int i = 0; i < maze.CellArrays.GetLength(0); i++)
            {
                Cell[] row = maze.CellArrays[i];

                for (int j = 0, length = row.Length; j < length; j++)
                {
                    Cell cell = row[j];
                    CellBehaviour cellBehaviour = Instantiate(cellPrefab, cellParent);
                    cellBehaviour.InitializeCell(cell);
                    cell.SetMonoBehaviourInstance(cellBehaviour);
                }
            }

            OnMazeInitialized?.Invoke(maze);
        }
    }
}
