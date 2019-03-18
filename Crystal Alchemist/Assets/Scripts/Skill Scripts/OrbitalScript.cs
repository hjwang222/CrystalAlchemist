using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitalScript : Script
{
    public override void onDestroy()
    {
       
    }

    public override void onUpdate()
    {
        if(this.skill.target != null) this.skill.transform.position = this.target.transform.position;
    }

    public override void onInitialize()
    {
        if (this.skill.target != null) this.skill.transform.position = this.target.transform.position;
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
