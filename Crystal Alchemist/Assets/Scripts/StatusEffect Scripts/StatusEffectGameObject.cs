using UnityEngine;


public class StatusEffectGameObject : MonoBehaviour
{
    [SerializeField]
    private Animator anim;

    private StatusEffect activeEffect;

    public void Initialize(StatusEffect effect)
    {
        this.activeEffect = effect;
    }

    public StatusEffect getEffect()
    {
        return this.activeEffect;
    }

    public void SetEnd()
    {
        if (this.anim != null) AnimatorUtil.SetAnimatorParameter(this.anim, "End");
        else DestroyIt();
    }

    public void DestroyIt()
    {
        Destroy(this.gameObject);
    }

    public void PlaySoundEffect(AudioClip audioClip)
    {
        AudioUtil.playSoundEffect(this.gameObject, audioClip);
    }
}
