﻿using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public enum StatusEffectActionType
{
    changeResource,
    ability,
    effect,
    destroy,
    speed,
    time,
    module,
    immortal,
    dispell
}

[System.Serializable]
public class StatusEffectAction
{
    public StatusEffectActionType actionType;

    [ShowIf("actionType", StatusEffectActionType.changeResource)]
    [SerializeField]
    private List<Price> resources;

    [ShowIf("actionType", StatusEffectActionType.speed)]
    [SerializeField]
    private float speed;

    [ShowIf("actionType", StatusEffectActionType.time)]
    [SerializeField]
    private float time;

    [ShowIf("actionType", StatusEffectActionType.ability)]
    [SerializeField]
    private List<Ability> abilities;

    [ShowIf("actionType", StatusEffectActionType.effect)]
    [SerializeField]
    private List<StatusEffect> effects;

    [ShowIf("actionType", StatusEffectActionType.dispell)]
    [SerializeField]
    private StatusEffectType StatusEffectType;

    public void DoAction(Character character, StatusEffect effect)
    {
        switch (this.actionType)
        {
            case StatusEffectActionType.changeResource: changeResource(character); break;
            case StatusEffectActionType.speed: character.updateSpeed(this.speed); break;
            case StatusEffectActionType.time: character.updateTimeDistortion(this.time); break;
            case StatusEffectActionType.module: effect.doModule(); break;
            case StatusEffectActionType.ability: useAbility(character); break;
            case StatusEffectActionType.immortal: if (character != null) character.setCannotDie(true); break;
            case StatusEffectActionType.destroy: effect.DestroyIt(); break;
            case StatusEffectActionType.dispell: DispellIt(character); break;
            case StatusEffectActionType.effect: AddStatusEffect(character); break;
        }
    }

    private void changeResource(Character character)
    {
        foreach (Price resource in this.resources)
        {
            character.updateResource(resource.resourceType, resource.item, resource.amount);
        }
    }

    private void useAbility(Character character)
    {
        foreach (Ability ability in this.abilities)
        {
            CustomUtilities.Skills.InstantiateAbility(ability);
        }
    }

    private void DispellIt(Character character)
    {
        if (this.StatusEffectType == StatusEffectType.debuff) CustomUtilities.StatusEffectUtil.RemoveAllStatusEffects(character.debuffs);
        else CustomUtilities.StatusEffectUtil.RemoveAllStatusEffects(character.buffs);
    }

    private void AddStatusEffect(Character character)
    {
        foreach (StatusEffect eff in this.effects)
        {
            CustomUtilities.StatusEffectUtil.AddStatusEffect(eff, character);
        }
    }

    public void ResetAction(Character character)
    {
        if (this.actionType == StatusEffectActionType.speed) character.updateSpeed(0);
        else if (this.actionType == StatusEffectActionType.time) character.updateTimeDistortion(0);
        else if (this.actionType == StatusEffectActionType.immortal) character.setCannotDie(false);
    }
}
