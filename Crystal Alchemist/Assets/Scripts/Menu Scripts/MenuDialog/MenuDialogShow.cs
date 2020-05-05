using UnityEngine;
using UnityEngine.Events;

public class MenuDialogShow : MonoBehaviour
{
    [SerializeField]
    private GameObject child;

    private void Start()
    {
        GameEvents.current.OnMenuDialogBox += Show;
    }

    private void OnDestroy()
    {
        GameEvents.current.OnMenuDialogBox -= Show;
    }

    private void Show(UnityEvent OnConfirm, Costs cost, string text, DialogBoxType type, MenuControls parent) => this.child.SetActive(true);
}
