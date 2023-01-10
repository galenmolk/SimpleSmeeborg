using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using DG;

namespace SimpleSmeeborg
{
    public class Character : MonoBehaviour
    {
        private Transform thisTransform;

        [Tooltip("Cell-to-cell tween duration in seconds")]
        [SerializeField] private float tweenDuration;

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

        private void HandlePathComplete(List<PathMarker> path)
        {
            StartCoroutine(TweenThroughMaze(path));
        }

        private IEnumerator TweenThroughMaze(List<PathMarker> path)
        {
            thisTransform.position = path[0].Cell.Position;

            for (int i = 0, count = path.Count; i < count; i++)
            {
                transform.DOMove(path[i].Cell.WorldPosition, tweenDuration);
                yield return tweenYield;
            }
        }
    }
}
