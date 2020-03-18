using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public enum StatusEffectActionType
{
    changeResource,
    stacks,
    ability,
    effect,
    destroy,
    speed,
    time,
    module,
    immortal
}

[System.Serializable]
public class StatusEffectAction
{
    public StatusEffectActionType actionType;

    [ShowIf("actionType", StatusEffectActionType.changeResource)]
    [SerializeField]
    private List<Price> resources;

    [ShowIf("actionType", StatusEffectActionType.stacks)]
    [SerializeField]
    private int amount;

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

    public void DoAction(Character character, StatusEffect effect)
    {
        if (this.actionType == StatusEffectActionType.changeResource && character != null)
        {
            foreach (Price resource in this.resources)
            {
                character.updateResource(resource.resourceType, resource.item, resource.amount);
            }
        }
        else if (this.actionType == StatusEffectActionType.speed)
        {
            character.updateSpeed(this.speed);
        }
        else if (this.actionType == StatusEffectActionType.time)
        {
            character.updateTimeDistortion(this.time);
        }
        else if (this.actionType == StatusEffectActionType.module)
        {
            effect.doModule();
        }
        else if (this.actionType == StatusEffectActionType.stacks)
        {
            CustomUtilities.StatusEffectUtil.RemoveStatusEffect(effect, false, character);
        }
        else if (this.actionType == StatusEffectActionType.ability)
        {
            foreach (Ability ability in this.abilities)
            {
                CustomUtilities.Skills.InstantiateAbility(ability);
            }
        }
        else if (this.actionType == StatusEffectActionType.immortal)
        {
            if (character != null) character.setCannotDie(true);
        }
        else if (this.actionType == StatusEffectActionType.destroy)
        {
            effect.DestroyIt();
        }
    }

    public void ResetAction(Character character)
    {
        if (this.actionType == StatusEffectActionType.speed) character.updateSpeed(0);
        else if (this.actionType == StatusEffectActionType.time) character.updateTimeDistortion(0);
        else if (this.actionType == StatusEffectActionType.immortal) character.setCannotDie(false);        
    }
}

