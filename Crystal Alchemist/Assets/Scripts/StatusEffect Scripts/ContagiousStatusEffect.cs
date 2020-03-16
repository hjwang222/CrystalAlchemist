﻿using UnityEngine;
using Sirenix.OdinInspector;

public class ContagiousStatusEffect : MonoBehaviour
{
    [BoxGroup("Statuseffekt Pflichtfelder")]
    [Required]
    [SerializeField]
    private BoxCollider2D effectCollider;

    private StatusEffect activeEffect;

    private void Start()
    {
        this.activeEffect = this.GetComponent<StatusEffectGameObject>().getEffect();
        this.effectCollider.size = this.activeEffect.getTarget().GetComponent<BoxCollider2D>().size;
        this.effectCollider.offset = this.activeEffect.getTarget().GetComponent<BoxCollider2D>().offset;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.isTrigger)
        {
            Character character = collision.GetComponent<Character>();
            if (character != null) CustomUtilities.StatusEffectUtil.AddStatusEffect(this.activeEffect, character);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.isTrigger)
        {
            Character character = collision.GetComponent<Character>();
            if (character != null) CustomUtilities.StatusEffectUtil.AddStatusEffect(this.activeEffect, character);
        }
    }
}
