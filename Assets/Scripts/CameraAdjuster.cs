using UnityEngine;

namespace SimpleSmeeborg
{
    /// <summary>
    /// CameraAdjuster moves its Transform to the maze centerpoint.
    /// It then sets the Camera's size to fit the maze within the viewport.
    /// </summary>
    [RequireComponent(typeof(Camera))]
    public class CameraAdjuster : MonoBehaviour
    {
        private Camera cam;
        private Transform thisTransform;

        private void Awake()
        {
            cam = GetComponent<Camera>();
            thisTransform = transform;
            MazeLoader.OnMazeInitialized += HandleMazeInitialized;
        }

        private void OnDestroy()
        {
            MazeLoader.OnMazeInitialized -= HandleMazeInitialized;
        }

        private void HandleMazeInitialized(Maze maze)
        {
            int mazeWidth = maze.Width;
            int mazeHeight = maze.Height;

            SetCameraPosition(mazeWidth, mazeHeight);
            SetCameraSize(mazeWidth, mazeHeight);
        }

        private void SetCameraPosition(int mazeWidth, int mazeHeight)
        {
            Vector3 cameraPosition = thisTransform.position;

            // Subtract 1 from both dimensions to account for 0-based indexing.
            cameraPosition.x = (mazeWidth - 1) * 0.5f;

            // Use a negative scalar to position 0,0 in the top left. 
            cameraPosition.y = (mazeHeight - 1) * -0.5f;
            thisTransform.position = cameraPosition;
        }

        private void SetCameraSize(int mazeWidth, int mazeHeight)
        {
            int largestDimension = Mathf.Max(mazeWidth, mazeHeight);
            cam.orthographicSize = largestDimension * 0.5f;
        }
    }
}
