using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ShowSolutionButton : MonoBehaviour
{
    [SerializeField] private UnityEvent onPressed;

    private Button button;
    
    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(InvokeOnPressed);
    }

    private void InvokeOnPressed()
    {
        onPressed?.Invoke();
    }
}
