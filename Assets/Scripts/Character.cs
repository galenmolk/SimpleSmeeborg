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

        private Transform thisTransform;
        private YieldInstruction tweenYield;

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
            StartCoroutine(TweenThroughMaze(path));
        }

        private IEnumerator TweenThroughMaze(List<PathNode> path)
        {
            thisTransform.position = path[0].WorldPosition;

            for (int i = 0, count = path.Count; i < count; i++)
            {
                transform.DOMove(path[i].WorldPosition, tweenDuration);
                yield return tweenYield;
            }
        }
    }
}
