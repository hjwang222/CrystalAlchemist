using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnAnimation : MonoBehaviour
{
    [SerializeField]
    private List<Animator> animators;    

    [SerializeField]
    private AudioSource audioSource;

    private Character character;
    private bool reverse;
    private bool resetStats = true;


    public void setCharacter(Character character)
    {
        this.setCharacter(character, false);
    }

    public void setStatReset(bool value)
    {
        this.resetStats = value;
    }

    public void setCharacter(Character character, bool reverse)
    {
        this.reverse = reverse;

        if (this.reverse)
        {
            foreach(Animator animator in this.animators)
            {
                CustomUtilities.UnityUtils.SetAnimatorParameter(animator, "Reverse");
            }
        }

        this.character = character;
        this.character.gameObject.SetActive(true);
        this.character.enableSpriteRenderer(true);
        this.character.enableScripts(false);
        this.character.initSpawn(this.resetStats);
        this.character.PlayRespawnAnimation();
    }

    public float getAnimationLength()
    {
        AnimationClip[] clips = this.animators[0].runtimeAnimatorController.animationClips;
        return clips[0].length;
    }

    public void DestroyIt()
    {
        if (!this.reverse)
        {
            this.character.enableScripts(true);
            this.character.removeColor(Color.white);
            //this.character.PlayRespawnAnimation(false);
        }

        Destroy(this.gameObject, 0.1f);
    }
}
