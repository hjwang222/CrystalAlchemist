using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchScript : Script
{
    public override void onDestroy()
    {

    }

    public override void onUpdate()
    {

    }

    public override void onInitialize()
    {
        this.GetComponent<BoxCollider2D>().offset = this.skill.sender.GetComponent<BoxCollider2D>().offset;
        this.GetComponent<BoxCollider2D>().size = this.skill.sender.GetComponent<BoxCollider2D>().size;
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
