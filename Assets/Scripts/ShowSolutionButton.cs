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

        private void InvokeOnPressed()
        {
            onPressed?.Invoke();
        }

        private void HandlePathFailed()
        {
            canvas.SetActive(false);
        }
    }
}
