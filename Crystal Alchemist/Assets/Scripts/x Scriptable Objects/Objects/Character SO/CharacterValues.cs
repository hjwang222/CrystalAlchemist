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
    public float mana;
    [Space(10)]
    [BoxGroup("Base Stats")]
    public float spellspeed;
    [BoxGroup("Base Stats")]
    public float speed;

    [BoxGroup("Base Attributes")]
    public float maxLife;
    [BoxGroup("Base Attributes")]
    public float maxMana;
    [Space(10)]
    [BoxGroup("Base Attributes")]
    public float lifeRegen;
    [BoxGroup("Base Attributes")]
    public float manaRegen;
    [Space(10)]
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
    [BoxGroup("Debug")]
    public float speedFactor = 5;

    public void Clear(CharacterStats stats)
    {
        this.maxLife = stats.maxLife;
        this.maxMana = stats.maxMana;
        this.lifeRegen = stats.lifeRegeneration;
        this.manaRegen = stats.manaRegeneration;
        this.buffPlus = stats.buffPlus;
        this.debuffMinus = stats.debuffMinus;

        this.life = stats.startLife;
        this.mana = stats.startMana;
        this.buffs.Clear();
        this.debuffs.Clear();
        this.speed = (stats.startSpeed / 100) * this.speedFactor;
        
        if (stats.lootTable != null) this.itemDrop = stats.lootTable.GetItemDrop();

        this.Initialize();
    }

    public void Initialize()
    {
        this.currentState = CharacterState.idle;
        foreach (Skill skill in this.activeSkills) skill.DeactivateIt();
        this.activeSkills.Clear();
    }

    #region Menu und DialogBox

    public bool CanUseAbilities()
    {
        if (this.currentState != CharacterState.interact
         && this.ActiveInField()) return true;
        return false;
    }

    public bool ActiveInField()
    {
        if (this.currentState != CharacterState.inDialog
            && this.currentState != CharacterState.respawning
            && this.currentState != CharacterState.inMenu
            && this.currentState != CharacterState.dead
            && !this.isCharacterStunned()) return true;
        return false;
    }

    public bool isCharacterStunned()
    {
        foreach (StatusEffect debuff in this.debuffs)
        {
            if (debuff.stunTarget) return true;
        }

        return false;
    }

    public void AddStatusEffect(StatusEffect effect)
    {
        if (effect.statusEffectType == StatusEffectType.debuff) this.debuffs.Add(effect);
        else this.buffs.Add(effect);
    }

    #endregion
}
