using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleIndicator : Indicator
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        /*
        if (this.skill.gameObject.activeInHierarchy)
        {
            this.transform.parent = this.skill.transform;
            this.transform.position = this.skill.transform.position;
        }
        else
        {
            //this.transform.parent = this.skill.sender.transform;
            this.transform.position = this.skill.sender.shadowRenderer.transform.position;
        }*/
    }    
    
}
