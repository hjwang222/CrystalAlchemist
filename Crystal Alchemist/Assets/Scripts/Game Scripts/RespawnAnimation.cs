using UnityEngine;

public class RespawnAnimation : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    private Character character;

    public void SpawnIn(Character character)
    {
        this.character = character;
        character.SpawnInWithAnimation(); //show Animation, but no actions possible
    }

    public void SpawnOut(Character character)
    {
        character.SpawnOut(); //no actions possible now
        AnimatorUtil.SetAnimatorParameter(animator, "Reverse");        
    }

    public float getAnimationLength()
    {
        AnimationClip[] clips = this.animator.runtimeAnimatorController.animationClips;
        return clips[0].length;
    }

    public void DestroyIt()
    {
        if(this.character != null) this.character.SpawnWithAnimationCompleted(); //spawn complete, actions possible!
        Destroy(this.gameObject, 0.1f);
    }
}
