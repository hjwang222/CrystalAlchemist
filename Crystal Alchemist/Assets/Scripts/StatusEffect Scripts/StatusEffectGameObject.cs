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
        CustomUtilities.UnityUtils.SetAnimatorParameter(this.anim, "End");
    }

    public void DestroyIt()
    {
        Destroy(this.gameObject);
    }

    public void PlaySoundEffect(AudioClip audioClip)
    {
        CustomUtilities.Audio.playSoundEffect(this.gameObject, audioClip);
    }
}
