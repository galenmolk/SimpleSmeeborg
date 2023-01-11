using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SimpleSmeeborg
{
    [RequireComponent(typeof(Button))]
    public class ShowSolutionButton : MonoBehaviour
    {
        [SerializeField] private UnityEvent onPressed;
        [SerializeField] private GameObject canvas;

        private Button button;

        private void Awake()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(InvokeOnPressed);
            FindPathAStar.OnPathFailed += HandlePathFailed;
        }

        private void OnDestroy()
        {
            FindPathAStar.OnPathFailed -= HandlePathFailed;
        }

        private void InvokeOnPressed()
        {
            onPressed?.Invoke();
        }

        private void HandlePathFailed()
        {
            // Hide the UI if no maze solution was found.
            canvas.SetActive(false);
        }
    }
}
