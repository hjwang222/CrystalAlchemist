using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleIndicator : Indicator
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        this.transform.parent = this.skill.transform;
        this.transform.position = this.skill.transform.position;
    }    
    
}
