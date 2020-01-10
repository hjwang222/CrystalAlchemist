using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnAnimation : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer characterSprite;

    [SerializeField]
    private SpriteRenderer characterSpriteBright;

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
                Utilities.UnityUtils.SetAnimatorParameter(animator, "Reverse");
            }
        }
        this.character = character;
        this.characterSprite.sprite = character.startSpriteForRespawn;
        this.characterSpriteBright.sprite = character.startSpriteForRespawnWhite;
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
            this.characterSprite.enabled = false;
            this.characterSpriteBright.enabled = false;
        
            character.gameObject.SetActive(true);
            character.initSpawn(this.resetStats);
        }

        Destroy(this.gameObject, 0.1f);
    }
}
