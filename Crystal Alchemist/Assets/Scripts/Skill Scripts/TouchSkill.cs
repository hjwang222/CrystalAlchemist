using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchSkill : StandardSkill
{
    public override void init()
    {
        base.init();        
        this.GetComponent<BoxCollider2D>().offset = this.sender.GetComponent<BoxCollider2D>().offset;
        this.GetComponent<BoxCollider2D>().size = this.sender.GetComponent<BoxCollider2D>().size;
    }
}
