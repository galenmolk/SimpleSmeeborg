using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SimpleSmeeborg
{
    [RequireComponent(typeof(Button))]
    public class ShowSolutionButton : MonoBehaviour
    {
        [SerializeField] private UnityEvent onPressed;
        [SerializeField] private CanvasGroup canvasGroup;

        private Button button;

        private void Awake()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(InvokeOnPressed);
            FindPathAStar.OnPathComplete += HandlePathComplete;
        }

        private void OnDestroy()
        {
            FindPathAStar.OnPathComplete -= HandlePathComplete;
        }

        private void InvokeOnPressed()
        {
            onPressed?.Invoke();
        }

        private void HandlePathComplete(List<PathNode> path)
        {
            canvasGroup.alpha = 1f;
        }
    }
}
