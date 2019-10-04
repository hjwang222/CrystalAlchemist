using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTrigger : MonoBehaviour
{
    [SerializeField]
    private Item item;

    [SerializeField]
    private float triggerLife = 1;

    [SerializeField]
    private List<StatusEffect> statuseffects = new List<StatusEffect>();


    public void checkTriggered(Character owner)
    {
        if (owner.life <= this.triggerLife)
        {
            foreach(StatusEffect effect in this.statuseffects)
            {
                Utilities.StatusEffectUtil.AddStatusEffect(effect, owner);
            }

            owner.updateResource(ResourceType.item, this.item, -1);
        }
    }


}
