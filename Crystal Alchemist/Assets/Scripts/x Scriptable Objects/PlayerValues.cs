using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerValues : ScriptableObject
{
    public List<StatusEffect> buffs = new List<StatusEffect>();
    public List<StatusEffect> debuffs = new List<StatusEffect>();

    public float life;
    public float maxLife;

    public float spellspeed;

    public float maxMana;
    public float mana;

    public float speed;
}

