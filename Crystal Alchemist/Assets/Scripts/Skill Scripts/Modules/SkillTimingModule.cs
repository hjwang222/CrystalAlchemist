using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class SkillTimingModule : SkillModule
{
    [BoxGroup("Start")]
    [SerializeField]
    private UnityEvent OnStart;

    [BoxGroup("Delay")]
    [SerializeField]
    private bool hasDelay;

    [BoxGroup("Delay")]
    [ShowIf("hasDelay")]
    [SerializeField]
    private float delay;

    [BoxGroup("Delay")]
    [ShowIf("hasDelay")]
    [SerializeField]
    private UnityEvent AfterDelay;

    [BoxGroup("Duration")]
    [SerializeField]
    private bool hasDuration;

    [BoxGroup("Duration")]
    [ShowIf("hasDuration")]
    [SerializeField]
    private float maxDuration;
       
    public override void Initialize()
    {
        if (this.hasDelay) StartCoroutine(delayCo());
        if (this.hasDuration) StartCoroutine(durationCo());

        this.OnStart?.Invoke();
    }

    private IEnumerator delayCo()
    {
        yield return new WaitForSeconds(this.delay);
        this.AfterDelay?.Invoke();
    }

    private IEnumerator durationCo()
    {
        yield return new WaitForSeconds(this.maxDuration);
        this.skill.DeactivateIt();
    }
}
