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
    private AudioSource audioSource;

    private Character character;

    public void setCharacter(Character character)
    {
        this.character = character;
        this.characterSprite.sprite = character.startSpriteForRespawn;
        this.characterSpriteBright.sprite = character.startSpriteForRespawnWhite;
    }

    public void DestroyIt()
    {
        this.characterSprite.enabled = false;
        this.characterSpriteBright.enabled = false;

        character.gameObject.SetActive(true);
        character.initSpawn();
        Destroy(this.gameObject, 0.1f);
    }

    public void PlayDeathSoundEffect(AudioClip soundEffect)
    {
        Utilities.Audio.playSoundEffect(this.audioSource, soundEffect);
    }

}
