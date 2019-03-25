using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blind : Script
{
    //STATUSEFFEKT SCRIPT "Blind"
    public GameObject instantiatNewGameObject;
    private GameObject panel;

    public override void onDestroy()
    {
        Animator anim = this.panel.transform.GetChild(0).GetComponent<Animator>();
        if (anim != null) anim.SetBool("Explode", true);
        Destroy(this.panel, 2f);
    }

    public override void onUpdate()
    {
        
    }

    public override void onInitialize()
    {
        this.panel = Instantiate(this.instantiatNewGameObject);
        //this.panel.hideFlags = HideFlags.HideInHierarchy;
    }

    public override void onExit(Collider2D hittedCharacter)
    {

    }

    public override void onStay(Collider2D hittedCharacter)
    {

    }

    public override void onEnter(Collider2D hittedCharacter)
    {

    }
}
