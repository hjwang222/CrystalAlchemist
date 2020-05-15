using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class OnCancelManager : MonoBehaviour
{
    [BoxGroup]
    [SerializeField]
    private UnityEvent onCancel;

    private bool inputPossible = false;

    private void OnEnable()
    { 
        if (GameEvents.current != null) GameEvents.current.OnCancel += OnCancel;
        else if (TitleScreenControls.current != null) TitleScreenControls.current.OnCancel += OnCancel;
        StartCoroutine(delayCo());
    }

    private void OnDisable()
    {
        if (GameEvents.current != null) GameEvents.current.OnCancel -= OnCancel;
        else if (TitleScreenControls.current != null) TitleScreenControls.current.OnCancel -= OnCancel;
    }

    public void OnCancel()
    {
        if (this.onCancel != null && this.inputPossible) this.onCancel.Invoke();
    }

    private IEnumerator delayCo()
    {
        this.inputPossible = false;
        yield return new WaitForSeconds(0.1f);
        this.inputPossible = true;
    }
}
