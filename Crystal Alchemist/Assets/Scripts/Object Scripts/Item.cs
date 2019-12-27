using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Sirenix.OdinInspector;
using UnityEditor;

#region Enums

public enum ItemFeature
{
    none,
    skills,
    map,
    hud
}

#endregion


public class Item : MonoBehaviour
{

    #region Attribute

    [Required]
    [SerializeField]
    [BoxGroup("Pflichtfeld")]
    private SpriteRenderer shadowRenderer;

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

    /*
    [FoldoutGroup("Item Texts", expanded: false)]
    [ShowIf("resourceType", ResourceType.item)]
    [SerializeField]
    public ItemFeature itemFeature = ItemFeature.none;*/

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
    public SimpleSignal keyItemSignal;

    [FoldoutGroup("Item Attributes", expanded: false)]
    [ShowIf("resourceType", ResourceType.special)]
    public GameObject specialObject;

    [FoldoutGroup("Item Attributes", expanded: false)]
    [ShowIf("resourceType", ResourceType.statuseffect)]
    public List<StatusEffect> statusEffects = new List<StatusEffect>();

    [FoldoutGroup("Sound", expanded: false)]
    public AudioClip collectSoundEffect;

    [FoldoutGroup("Signals", expanded: false)]
    public SimpleSignal signal;

    private AudioSource audioSource;
    private Animator anim;

    #endregion


    #region Start Funktionen

    private void Awake()
    {
        //TODO: set Sprite if Skill != null
        init();
    }

    private void init()
    {
        this.audioSource = this.transform.gameObject.AddComponent<AudioSource>();
        this.audioSource.loop = false;
        this.audioSource.playOnAwake = false;
        this.anim = this.GetComponent<Animator>();
        if (this.itemSpriteInventory == null) this.itemSpriteInventory = this.itemSprite;
        //this.soundEffects = this.GetComponents<AudioSource>();
    }

    private void Start()
    {
        //Check if keyItem already in Inventory

        if (this.isKeyItem)
        {
            Player player = GameObject.FindWithTag("Player").GetComponent<Player>();
            if (player != null && CustomUtilities.Items.getAmountFromInventory(this,player.inventory,false) > 0) Destroy(this.gameObject);
        }
    }

    #endregion


    public void playSounds()
    {
        CustomUtilities.Audio.playSoundEffect(this.audioSource, this.collectSoundEffect);        
    }

    public int getTotalAmount()
    {
        return this.value * this.amount;
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
        if(this.shadowRenderer != null) this.shadowRenderer.enabled = false;
    }

    #endregion


    #region Collect Item Funktionen

    public void OnTriggerEnter2D(Collider2D character)
    {
        if (!character.isTrigger)
        {
            Character chara = character.GetComponent<Character>();
            if (chara != null) chara.collect(this, true);
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
