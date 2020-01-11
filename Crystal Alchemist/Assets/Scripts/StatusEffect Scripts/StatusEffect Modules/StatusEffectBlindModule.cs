using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectBlindModule : StatusEffectModule
{
    public GameObject instantiatNewGameObject;
    private GameObject panel;

    public override void doAction()
    {
        this.panel = Instantiate(this.instantiatNewGameObject);
    }

    private void OnDestroy()
    {
        Animator animator = this.panel.transform.GetChild(0).GetComponent<Animator>();
        CustomUtilities.UnityUtils.SetAnimatorParameter(animator, "Explode", true);
        Destroy(this.panel, 2f);
    }
}
