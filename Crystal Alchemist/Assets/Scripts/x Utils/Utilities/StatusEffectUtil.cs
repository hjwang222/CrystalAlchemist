using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectUtil : MonoBehaviour
{
    public static bool isCharacterStunned(Character character)
    {
        foreach (StatusEffect debuff in character.values.debuffs)
        {
            if (debuff.stunTarget) return true;
        }

        return false;
    }

    public static void RemoveAllStatusEffects(List<StatusEffect> statusEffects)
    {
        List<StatusEffect> dispellStatusEffects = new List<StatusEffect>();

        //Store in temp List to avoid Enumeration Exception
        foreach (StatusEffect effect in statusEffects)
        {
            dispellStatusEffects.Add(effect);
        }

        foreach (StatusEffect effect in dispellStatusEffects)
        {
            effect.DestroyIt();
        }

        dispellStatusEffects.Clear();
    }

    public static void RemoveStatusEffect(StatusEffect statusEffect, bool allTheSame, Character character)
    {
        List<StatusEffect> statusEffects = null;
        List<StatusEffect> dispellStatusEffects = new List<StatusEffect>();

        if (statusEffect.statusEffectType == StatusEffectType.debuff) statusEffects = character.values.debuffs;
        else if (statusEffect.statusEffectType == StatusEffectType.buff) statusEffects = character.values.buffs;

        //Store in temp List to avoid Enumeration Exception
        foreach (StatusEffect effect in statusEffects)
        {
            if (effect.statusEffectName == statusEffect.statusEffectName)
            {
                dispellStatusEffects.Add(effect);
                if (!allTheSame) break;
            }
        }

        foreach (StatusEffect effect in dispellStatusEffects)
        {
            effect.DestroyIt();
        }

        dispellStatusEffects.Clear();
    }

    public static void AddStatusEffect(StatusEffect statusEffect, Character character)
    {
        if (statusEffect != null && character.stats.characterType != CharacterType.Object)
        {
            bool isImmune = false;

            if (character.stats.isImmuneToAllDebuffs
                && statusEffect.statusEffectType == StatusEffectType.debuff) isImmune = true;
            else
            {
                for (int i = 0; i < character.stats.immunityToStatusEffects.Count; i++)
                {
                    StatusEffect immunityEffect = character.stats.immunityToStatusEffects[i];
                    if (statusEffect.statusEffectName == immunityEffect.statusEffectName)
                    {
                        isImmune = true;
                        break;
                    }
                }
            }

            if (!isImmune)
            {
                List<StatusEffect> statusEffects = null;
                List<StatusEffect> result = new List<StatusEffect>();

                //add to list for better reference
                if (statusEffect.statusEffectType == StatusEffectType.debuff) statusEffects = character.values.debuffs;
                else if (statusEffect.statusEffectType == StatusEffectType.buff) statusEffects = character.values.buffs;

                for (int i = 0; i < statusEffects.Count; i++)
                {
                    if (statusEffects[i].statusEffectName == statusEffect.statusEffectName)
                    {
                        //Hole alle gleichnamigen Effekte aus der Liste
                        result.Add(statusEffects[i]);
                    }
                }

                //TODO, das geht noch besser
                if (result.Count < statusEffect.maxStacks)
                {
                    //Wenn der Effekte die maximale Anzahl Stacks nicht überschritten hat -> Hinzufügen
                    instantiateStatusEffect(statusEffect, character);
                }
                else
                {
                    if (statusEffect.canOverride)
                    {
                        //Wenn der Effekt überschreiben kann, soll der Effekt mit der kürzesten Dauer entfernt werden
                        StatusEffect toDestroy = result[0];
                        toDestroy.DestroyIt();

                        instantiateStatusEffect(statusEffect, character);
                    }
                    else if (statusEffect.canDeactivateIt)
                    {
                        StatusEffect toDestroy = result[0];
                        toDestroy.DestroyIt();
                    }
                }
            }
        }
    }

    private static void instantiateStatusEffect(StatusEffect statusEffect, Character character)
    {
        StatusEffect statusEffectClone = Instantiate(statusEffect);
        statusEffectClone.Initialize(character);
    }
}
