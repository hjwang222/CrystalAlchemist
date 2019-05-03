using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public struct affectedResource
{
    public ResourceType resourceType;

    [Range(-Utilities.maxFloatInfinite, Utilities.maxFloatInfinite)]
    public float amount;
}

public class affectMana : StatusEffect
{
    #region Attributes
    public List<affectedResource> affectedResources;
    #endregion


    #region Overrides
    public override void doEffect()
    {
        foreach (affectedResource resource in this.affectedResources)
        {
            base.doEffect();
            this.target.updateResource(resource.resourceType, resource.amount);
        }
    }
    #endregion
}
