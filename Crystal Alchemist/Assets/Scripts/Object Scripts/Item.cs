﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Sirenix.OdinInspector;
using UnityEditor;

public class Item : MonoBehaviour
{
    [Required]
    [BoxGroup("Pflichtfeld")]
    public SpriteRenderer shadowRenderer;

    [Required]
    [BoxGroup("Pflichtfeld")]
    public Sprite itemSprite;

    [Required]
    [BoxGroup("Pflichtfeld")]
    public Sprite itemSpriteInventory;

    [FoldoutGroup("Item Texts", expanded: false)]
    [SerializeField]
    public string itemName;

    [FoldoutGroup("Item Texts", expanded: false)]
    [Tooltip("Beschreibung des Skills")]
    [TextArea]
    public string itemDescription;

    [FoldoutGroup("Item Texts", expanded: false)]
    [ShowIf("resourceType", ResourceType.item)]
    [SerializeField]
    public string itemGroup;

    [Tooltip("Slot-Nummer im Inventar. Wenn -1 dann kein Platz im Inventar")]
    [FoldoutGroup("Item Texts", expanded: false)]
    [SerializeField]
    public int itemSlot = -1;

    [Space(10)]
    [FoldoutGroup("Item Texts", expanded: false)]
    [SerializeField]
    public string itemNameEnglish;

    [FoldoutGroup("Item Texts", expanded: false)]
    [Tooltip("Beschreibung des Skills")]
    [TextArea]
    public string itemDescriptionEnglish;

    [FoldoutGroup("Item Texts", expanded: false)]
    [SerializeField]
    public string itemGroupEnglish;

    [FoldoutGroup("Item Attributes", expanded: false)]
    public int amount = 1;

    [FoldoutGroup("Item Attributes", expanded: false)]
    public int value = 1;

    [FoldoutGroup("Item Attributes", expanded: false)]
    public int maxAmount;

    [FoldoutGroup("Item Attributes", expanded: false)]
    [EnumToggleButtons]
    public ResourceType resourceType;

    [FoldoutGroup("Item Attributes", expanded: false)]
    [ShowIf("resourceType", ResourceType.item)]
    public bool isKeyItem = false;

    [FoldoutGroup("Item Attributes", expanded: false)]
    [ShowIf("isKeyItem")]
    public bool useItemGroup = false;

    [FoldoutGroup("Item Attributes", expanded: false)]
    [ShowIf("isKeyItem")]
    public SimpleSignal keyItemSignal;

    [FoldoutGroup("Item Attributes", expanded: false)]
    [ShowIf("resourceType", ResourceType.skill)]
    public Ability ability;

    [FoldoutGroup("Item Attributes", expanded: false)]
    [ShowIf("resourceType", ResourceType.statuseffect)]
    public List<StatusEffect> statusEffects = new List<StatusEffect>();

    [FoldoutGroup("Sound", expanded: false)]
    public AudioClip collectSoundEffect;

    [FoldoutGroup("Signals", expanded: false)]
    public SimpleSignal signal;


    private Animator anim;

    #region Start Funktionen

    private void Awake()
    {
        init();
    }

    public int getTotalAmount()
    {
        return this.value * this.amount;
    }

    private void init()
    {
        this.anim = this.GetComponent<Animator>();
        if (this.itemSpriteInventory == null) this.itemSpriteInventory = this.itemSprite;
    }

    private void Start()
    {
        //Check if keyItem already in Inventory
        if (checkIfAlreadyThere()) Destroy(this.gameObject);
    }

    public bool checkIfAlreadyThere()
    {
        if (this.isKeyItem)
        {
            Player player = GameObject.FindWithTag("Player").GetComponent<Player>();
            if (player != null && CustomUtilities.Items.hasKeyItemAlready(this, player.inventory)) return true;            
        }

        return false;
    }

    #endregion


    public void playSounds()
    {
        CustomUtilities.Audio.playSoundEffect(this.gameObject, this.collectSoundEffect);        
    }

    #region Collect Item Funktionen

    public void OnTriggerEnter2D(Collider2D character)
    {
        if (!character.isTrigger)
        {
            Character chara = character.GetComponent<Character>();
            if (chara != null)
            {
                chara.collect(this, true);

                if(chara.GetComponent<Player>() != null && this.GetComponent<DialogSystem>() != null)
                    this.GetComponent<DialogSystem>().show(chara.GetComponent<Player>(), this);
            }
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
