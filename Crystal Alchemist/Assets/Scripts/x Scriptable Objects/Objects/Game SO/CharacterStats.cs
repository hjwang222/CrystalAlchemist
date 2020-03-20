using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


#region Enums
public enum CharacterState
{
    walk,
    attack,
    defend,
    interact, //in Reichweite eines interagierbaren Objektes
    inDialog, //Dialog-Box ist offen
    inMenu, //Pause oder Inventar ist offen
    knockedback, //im Knockback
    idle,
    silent, //kann nicht angreifen
    dead,
    manually,
    respawning
}

public enum CharacterType
{
    Player,
    Enemy,
    NPC,
    Object
}

public enum Gender
{
    male,
    female,
    none
}

#endregion

[CreateAssetMenu(menuName = "Game/CharacterStats")]
public class CharacterStats : ScriptableObject
{
    [Required]
    [BoxGroup("Pflichtfelder")]
    public string characterName;

    [BoxGroup("Pflichtfelder")]
    public string englischCharacterName;

    ////////////////////////////////////////////////////////////////

    [TabGroup("Start-Values")]
    [Tooltip("Leben, mit dem der Spieler startet")]
    public float startLife = 1;

    [TabGroup("Start-Values")]
    [Tooltip("Mana, mit dem der Spieler startet")]
    public float startMana = 1;

    [TabGroup("Start-Values")]
    [Tooltip("Movement-Speed in %, mit dem der Spieler startet")]
    public float startSpeed = 100;

    [TabGroup("Start-Values")]
    [Tooltip("Geschwindigkeitsmodifier in % von Cooldown und Castzeit")]
    public float startSpellSpeed = 100;

    [TabGroup("Start-Values")]
    public bool isMassive = false;

    [Space(10)]
    [TabGroup("Start-Values")]
    [Tooltip("Immunität von Statuseffekten")]
    public bool isImmuneToAllDebuffs = false;
    
    [TabGroup("Start-Values")]
    [HideIf("isImmuneToAllDebuffs")]
    [Tooltip("Immunität von Statuseffekten")]
    public List<StatusEffect> immunityToStatusEffects = new List<StatusEffect>();


    [Space(10)]
    [TabGroup("Spawn Values")]
    [Tooltip("Respawn-Zeit")]
    public bool hasRespawn = true;

    [TabGroup("Spawn Values")]
    [Tooltip("Respawn-Zeit")]
    [ShowIf("hasRespawn")]
    public float respawnTime = 30;

    [TabGroup("Spawn Values")]
    [Tooltip("Respawn-Animation")]
    public RespawnAnimation respawnAnimation;

    [TabGroup("Spawn Values")]
    [Tooltip("Respawn-Animation")]
    public DeathAnimation deathAnimation;


    ////////////////////////////////////////////////////////////////

    [FoldoutGroup("Upgrades", expanded: false)]
    [Tooltip("Maximales Life")]
    public float maxLife = 1;

    [FoldoutGroup("Upgrades", expanded: false)]
    [Tooltip("Maximales Mana")]
    public float maxMana = 1;

    [Space(10)]
    [FoldoutGroup("Upgrades", expanded: false)]
    [Tooltip("Höhe der Lebensregeneration")]
    public float lifeRegeneration = 0;

    [FoldoutGroup("Upgrades", expanded: false)]
    [Tooltip("Höhe der Manaregeneration")]
    public float manaRegeneration = 0;

    [FoldoutGroup("Upgrades", expanded: false)]
    [Tooltip("Intervall der Regeneration")]
    [Range(0, 3)]
    public float regenerationInterval = 0;

    [Space(10)]
    [FoldoutGroup("Upgrades", expanded: false)]
    [Range(0,100)]
    [Tooltip("Verlängerung von Buffs in Prozent")]
    public int buffPlus = 0;

    [FoldoutGroup("Upgrades", expanded: false)]
    [Range(-100, 0)]
    [Tooltip("Verkürzung von Debuffs in Prozent")]
    public int debuffMinus = 0;

    ////////////////////////////////////////////////////////////////

    [FoldoutGroup("Schaden", expanded: false)]
    [Required]
    [Tooltip("DamageNumber-Objekt hier rein (nur für zerstörbare Objekte)")]
    public GameObject damageNumber;

    [Space(10)]
    [FoldoutGroup("Schaden", expanded: false)]
    [Tooltip("Wie stark (-) oder schwach (+) kann das Objekt zurück gestoßen werden?")]
    public float antiKnockback = 0;

    [FoldoutGroup("Schaden", expanded: false)]
    [Tooltip("Unverwundbarkeitszeit")]
    [Range(0, 10)]
    public float cannotBeHitTime = 0.3f;

    [Space(10)]
    [FoldoutGroup("Schaden", expanded: false)]
    [Tooltip("Farbe, wenn Gegner getroffen wurde")]
    public bool showHitcolor = true;

    [FoldoutGroup("Schaden", expanded: false)]
    [ShowIf("showHitcolor")]
    public Color hitColor = Color.white;


    ////////////////////////////////////////////////////////////////


    [FoldoutGroup("Loot", expanded: false)]
    [Tooltip("Items und deren Wahrscheinlichkeit zwischen 1 und 100")]
    public LootTable lootTable;


    ////////////////////////////////////////////////////////////////


    [FoldoutGroup("Sound", expanded: false)]
    [Tooltip("Soundeffekt, wenn Gegner getroffen wurde")]
    public AudioClip hitSoundEffect;


    ////////////////////////////////////////////////////////////////


    [FoldoutGroup("RPG Elements", expanded: false)]
    [Tooltip("Rasse")]
    public string characterSpecies;

    [FoldoutGroup("RPG Elements", expanded: false)]
    [Tooltip("Geschlecht")]
    [EnumToggleButtons]
    public Gender characterGender = Gender.none;

    [FoldoutGroup("RPG Elements", expanded: false)]
    [Tooltip("Um welchen Typ handelt es sich?")]
    [EnumToggleButtons]
    public CharacterType characterType = CharacterType.Object;

}
