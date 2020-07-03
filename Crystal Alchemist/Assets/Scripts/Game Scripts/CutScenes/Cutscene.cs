using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class Cutscene : MonoBehaviour
{
    [SerializeField]
    [TextArea]
    private string notes;

    [SerializeField]
    private BoolValue CutSceneValue;

    [SerializeField]
    private bool hasDuration = true;

    [ShowIf("hasDuration")]
    [SerializeField]
    private float maxDuration = 10f;

    [SerializeField]
    private UnityEvent OnStart;

    [SerializeField]
    private UnityEvent OnEnd;

    [ButtonGroup]
    public void Play() => Invoke("PlayIt", 0.1f);


    private void OnValidate()
    {
        if (!this.hasDuration) this.maxDuration = 0;
    }

    private void PlayIt()
    {
        float duration = this.maxDuration;
        if (!this.hasDuration) duration = 0;
        this.CutSceneValue.setValue(true);
        GameEvents.current.DoCutScene();
        this.OnStart?.Invoke();
        Invoke("Completed", duration);
    }

    [ButtonGroup]
    public void Completed()
    {        
        this.CutSceneValue.setValue(false);
        GameEvents.current.DoCutScene();
        this.OnEnd?.Invoke();
    }

    public void RaiseSignal(SimpleSignal signal) => signal?.Raise();
}
