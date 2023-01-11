using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace SimpleSmeeborg
{
    /// <summary>
    /// Character listens for the maze solution, then tweens along the path.
    /// </summary>
    public class Character : MonoBehaviour
    {
        [Tooltip("Time in seconds for character to traverse between two cells.")]
        [SerializeField] private float tweenDuration;
        [SerializeField] private GameObject graphics;

        private Transform thisTransform;
        private YieldInstruction tweenYield;

        private List<PathNode> mazeSolution;

        public void FollowPath()
        {
            if (mazeSolution != null && mazeSolution.Count > 1)
            {
                StartCoroutine(TweenThroughMaze(mazeSolution));
            }
        }

        private void Awake()
        {
            thisTransform = transform;
            tweenYield = new WaitForSeconds(tweenDuration);
            FindPathAStar.OnPathComplete += HandlePathComplete;
        }

        private void OnDestroy()
        {
            FindPathAStar.OnPathComplete -= HandlePathComplete;
        }

        private void HandlePathComplete(List<PathNode> path)
        {
            mazeSolution = path;
        }

        private IEnumerator TweenThroughMaze(List<PathNode> path)
        {
            thisTransform.position = path[0].WorldPosition;
            graphics.SetActive(true);

            for (int i = 0, count = path.Count; i < count; i++)
            {
                yield return tweenYield;
                transform.DOMove(path[i].WorldPosition, tweenDuration);
            }
        }
    }
}
