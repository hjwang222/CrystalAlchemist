using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class ContagiousStatusEffect : MonoBehaviour
{
    [BoxGroup("Statuseffekt Pflichtfelder")]
    [Required]
    [SerializeField]
    private BoxCollider2D effectCollider;

    [BoxGroup("Statuseffekt Pflichtfelder")]
    [Required]
    [SerializeField]
    private StatusEffect statuseffect;

    private void Start()
    {
        this.effectCollider.size = this.statuseffect.target.GetComponent<BoxCollider2D>().size;
        this.effectCollider.offset = this.statuseffect.target.GetComponent<BoxCollider2D>().offset;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.isTrigger)
        {
            Character character = collision.GetComponent<Character>();
            if (character != null) CustomUtilities.StatusEffectUtil.AddStatusEffect(this.statuseffect, character);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.isTrigger)
        {
            Character character = collision.GetComponent<Character>();
            if (character != null) CustomUtilities.StatusEffectUtil.AddStatusEffect(this.statuseffect, character);
        }
    }
}
