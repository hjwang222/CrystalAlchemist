using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "Game/Characters/Character Values")]
public class CharacterValues : ScriptableObject
{
    [BoxGroup]
    public bool isPlayer = false;

    [BoxGroup("Base Stats")]
    public float life;
    [BoxGroup("Base Stats")]
    public float spellspeed;
    [BoxGroup("Base Stats")]
    public float mana;
    [BoxGroup("Base Stats")]
    public float speed;

    [BoxGroup("Base Attributes")]
    public float maxLife;
    [BoxGroup("Base Attributes")]
    public float maxMana;
    [BoxGroup("Base Attributes")]
    public float lifeRegen;
    [BoxGroup("Base Attributes")]
    public float manaRegen;
    [BoxGroup("Base Attributes")]
    public int buffPlus;
    [BoxGroup("Base Attributes")]
    public int debuffMinus;

    [BoxGroup("States")]
    public CharacterState currentState;
    [BoxGroup("States")]
    public bool cantBeHit;
    [BoxGroup("States")]
    public bool isInvincible = false;

    [BoxGroup("States")]
    public Vector2 direction;
    [BoxGroup("States")]
    public bool lockAnimation = false;
    [BoxGroup("States")]
    public float timeDistortion = 1;
    [BoxGroup("States")]
    public float steps = 0;
    [BoxGroup("States")]
    public bool isOnIce = false;

    [BoxGroup("Debug")]
    public List<StatusEffect> buffs = new List<StatusEffect>();
    [BoxGroup("Debug")]
    public List<StatusEffect> debuffs = new List<StatusEffect>();
    [BoxGroup("Debug")]
    public List<Character> activePets = new List<Character>();
    [BoxGroup("Debug")]
    public List<Skill> activeSkills = new List<Skill>();
    [BoxGroup("Debug")]
    public ItemDrop itemDrop;

    public void SetAttributes(CharacterStats stats)
    {
        this.maxLife = stats.maxLife;
        this.maxMana = stats.maxMana;
        this.lifeRegen = stats.lifeRegeneration;
        this.manaRegen = stats.manaRegeneration;
        this.buffPlus = stats.buffPlus;
        this.debuffMinus = stats.debuffMinus;
    }

    public void ResetValues(CharacterStats stats, float speedFactor)
    {
        this.life = stats.startLife;
        this.mana = stats.startMana;
        this.buffs.Clear();
        this.debuffs.Clear();
        this.speed = (stats.startSpeed / 100) * speedFactor;
    }

    public void ClearValues(CharacterStats stats)
    {
        this.currentState = CharacterState.idle;
        if (stats.lootTable != null) this.itemDrop = stats.lootTable.GetItemDrop();

        foreach (Skill skill in this.activeSkills) skill.DeactivateIt();   
        this.activeSkills.Clear();
    }
}
