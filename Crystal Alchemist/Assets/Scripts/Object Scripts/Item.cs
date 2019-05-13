using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Sirenix.OdinInspector;

#region Enums

public enum ItemGroup
{
    wood,
    stone,
    metal,
    key,
    coin,
    crystal,
    skill
}

//Resource = Mana oder Life
//Rest = Items

#endregion

public class Item : MonoBehaviour
{

    #region Attribute

    [FoldoutGroup("Item Attributes", expanded: false)]
    public string itemName;

    [FoldoutGroup("Item Attributes", expanded: false)]
    public int amount;

    [FoldoutGroup("Item Attributes", expanded: false)]
    public int maxAmount;
       
    [FoldoutGroup("Item Attributes", expanded: false)]
    [EnumToggleButtons]
    public ResourceType resourceType;

    [FoldoutGroup("Item Attributes", expanded: false)]
    [ShowIf("resourceType", ResourceType.item)]
    [EnumToggleButtons]
    public ItemGroup itemGroup;

    [FoldoutGroup("Sound", expanded: false)]
    public AudioClip collectSoundEffect;

    [FoldoutGroup("Sound", expanded: false)]
    public AudioClip raiseSoundEffect;

    private AudioSource audioSource;
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

    public void playSounds()
    {
        Utilities.playSoundEffect(this.audioSource, this.collectSoundEffect);
        Utilities.playSoundEffect(this.audioSource, this.raiseSoundEffect);
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
