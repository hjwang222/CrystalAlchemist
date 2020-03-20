using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SkillAffectStatusEffects : SkillExtension
{
    [SerializeField]
    private StatusEffectType affectAllOfKind;

    [SerializeField]
    private bool dispellIt = false;

    [SerializeField]
    private bool allTheSame = false;

    [SerializeField]
    [Range(0, 100)]
    private int extendTimePercentage = 0;

    private void Start()
    {
        changeStatusEffects(this.skill.sender);
    }

    private void changeStatusEffects(Character character)
    {
        List<StatusEffect> changeEffects = new List<StatusEffect>();

        //all effects of a kind
        if (this.affectAllOfKind == StatusEffectType.buff)
        {
            changeEffects.AddRange(character.buffs);
        }
        else
        {
            changeEffects.AddRange(character.debuffs);
        }

        if (changeEffects.Count > 0)
        {
            if (this.allTheSame)
            {
                foreach (StatusEffect effect in changeEffects)
                {
                    if (this.dispellIt) StatusEffectUtil.RemoveStatusEffect(effect, false, character);
                    if (this.extendTimePercentage > 0) effect.changeTime(extendTimePercentage);
                }
            }
            else
            {
                if (this.dispellIt) StatusEffectUtil.RemoveStatusEffect(changeEffects[0], false, character);
                if (this.extendTimePercentage > 0) changeEffects[0].changeTime(extendTimePercentage);
            }
        }
    }
}
