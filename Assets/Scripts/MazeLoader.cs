using UnityEngine;

namespace SimpleSmeeborg
{
    public class MazeLoader : MonoBehaviour
    {
        [SerializeField] private TextAsset inputAscii;
        [SerializeField] private RectTransform cellParent;
        [SerializeField] private CellBehaviour cellPrefab;

        private string asciiString;

        private void Start()
        {
            asciiString = inputAscii.text;

            Maze maze = new Maze();

            maze.InitializeMaze(asciiString);

            for (int i = 0; i < maze.CellMatrix.GetLength(0); i++)
            {
                Cell[] row = maze.CellMatrix[i];

                for (int j = 0, length = row.Length; j < length; j++)
                {
                    Cell c = row[j];
                    CellBehaviour cellBehaviour = Instantiate(cellPrefab, cellParent);
                    cellBehaviour.InitializeCell(c);
                }
            }
        }
    }
}
