using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class SkillMechanicHit : SkillHitTrigger
{
    [BoxGroup("Action")]
    [SerializeField]
    private UnityEvent AfterDelay;

    [BoxGroup("Action")]
    [SerializeField]
    [Range(0, 20)]
    private float delay = 3f;

    public virtual void Start() => StartCoroutine(delayCo());

    private IEnumerator delayCo()
    {
        yield return new WaitForSeconds(this.delay);
        this.AfterDelay?.Invoke();
    }

    public void PlayAnimation(string trigger)
    {
        AnimatorUtil.SetAnimatorParameter(this.GetComponent<Animator>(), trigger);
    }
}
