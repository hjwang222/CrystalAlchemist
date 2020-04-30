using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnAnimation : MonoBehaviour
{
    [SerializeField]
    private Animator animator;    

    private Character character;
    private bool reverse;

    public void resetCharacter(Character character)
    {
        this.setCharacter(character, false, true);
    }

    public void setCharacter(Character character, bool reverse, bool reset)
    {
        this.reverse = reverse;
        this.character = character;

        if (this.reverse)
        {
            this.character.SpawnOut();
            AnimatorUtil.SetAnimatorParameter(animator, "Reverse");            
        }
        else this.character.SpawnInWithAnimation(reset); //show Character but no Actions!     
    }

    public float getAnimationLength()
    {
        AnimationClip[] clips = this.animator.runtimeAnimatorController.animationClips;
        return clips[0].length;
    }

    public void DestroyIt()
    {
        if (!this.reverse) this.character.SpawnWithAnimationCompleted(); //spawn complete, now ACTION!
        Destroy(this.gameObject, 0.1f);
    }
}
