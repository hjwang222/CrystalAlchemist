using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class affectResourceStatusEffect : StatusEffect
{
    #region Attributes
    [FoldoutGroup("affect Resource", expanded: false)]
    [SerializeField]
    private List<affectedResource> affectedResources;

    #endregion


    #region Overrides
    public override void doEffect()
    {
        foreach (affectedResource resource in this.affectedResources)
        {
            base.doEffect();
            this.target.updateResource(resource.resourceType, resource.item, resource.amount);
        }
    }
    #endregion
}
