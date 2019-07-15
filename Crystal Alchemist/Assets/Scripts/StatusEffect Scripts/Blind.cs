using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blind : StatusEffect
{
    //STATUSEFFEKT SCRIPT "Blind"
    public GameObject instantiatNewGameObject;
    private GameObject panel;

    public override void DestroyIt()
    {
        Animator animator = this.panel.transform.GetChild(0).GetComponent<Animator>();
        Utilities.UnityUtils.SetAnimatorParameter(animator, "Explode", true);
        Destroy(this.panel, 2f);
        base.DestroyIt();
    }

    public override void init()
    {
        base.init();
        this.panel = Instantiate(this.instantiatNewGameObject);
        //this.panel.hideFlags = HideFlags.HideInHierarchy;
    }
}
