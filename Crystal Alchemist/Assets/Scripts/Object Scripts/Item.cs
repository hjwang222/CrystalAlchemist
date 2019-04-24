using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

#region Enums

public enum ItemType
{
    crystal,
    coin,
    key,
    health,
    mana,
    bosskey
}

public enum Rarity
{
    common,
    uncommon,
    rare,
    epic,
    legendary
}

#endregion

public class Item : MonoBehaviour
{

    #region Attribute

    public int amount;
    public string itemName;
    public ItemType itemType;
    public Rarity rarity;

    public AudioSource audioSource;

    public AudioClip collectSoundEffect;
    public AudioClip raiseSoundEffect;
    public FloatValue effectVolume;

    private Animator anim;

    #endregion


    #region Start Funktionen

    private void Awake()
    {
        init();
    }

    private void init()
    {
        this.audioSource = this.transform.gameObject.AddComponent<AudioSource>();
        this.audioSource.loop = false;
        this.audioSource.playOnAwake = false;
        this.anim = this.GetComponent<Animator>();
        //this.soundEffects = this.GetComponents<AudioSource>();
    }

    #endregion


    public Sprite getSprite()
    {
        return this.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
    }


    #region Treasure specific Function
    public void showFromTreasure()
    {
        //Als Kisten-Item darf es nicht einsammelbar sein und muss als Position die Kiste haben

        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 0.5f);
        SortingGroup group = this.GetComponent<SortingGroup>();
        if (group != null) group.sortingOrder = 1;
        this.GetComponent<BoxCollider2D>().enabled = false;   
        this.anim.enabled = true;  
    }

    #endregion


    #region Collect Item Funktionen

    public void OnTriggerEnter2D(Collider2D character)
    {
        if (character.CompareTag("Player") && !character.isTrigger)
        {
            Player player = character.GetComponent<Player>();
            if (player != null) player.collect(this, true);
        }
    }   

    public void DestroyIt()
    {
        this.GetComponent<BoxCollider2D>().enabled = false;
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        Destroy(this.gameObject, 2f);
    }
        #endregion
}
