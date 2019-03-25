using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blind : StatusEffect
{
    //STATUSEFFEKT SCRIPT "Blind"
    public GameObject instantiatNewGameObject;
    private GameObject panel;

    public new void DestroyIt()
    {
        Animator anim = this.panel.transform.GetChild(0).GetComponent<Animator>();
        if (anim != null) anim.SetBool("Explode", true);
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
