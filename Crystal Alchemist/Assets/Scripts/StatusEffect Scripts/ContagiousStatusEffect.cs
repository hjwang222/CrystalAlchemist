using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class ContagiousStatusEffect : affectResourceStatusEffect
{
    [BoxGroup("Statuseffekt Pflichtfelder")]
    [Required]
    [SerializeField]
    private BoxCollider2D collider;

    public override void init()
    {
        base.init();
        this.collider.size = this.target.GetComponent<BoxCollider2D>().size;
        this.collider.offset = this.target.GetComponent<BoxCollider2D>().offset;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.isTrigger)
        {
            Character character = collision.GetComponent<Character>();
            if (character != null) character.AddStatusEffect(this);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.isTrigger)
        {
            Character character = collision.GetComponent<Character>();
            if (character != null) character.AddStatusEffect(this);
        }
    }
}
