using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchSkill : StandardSkill
{
    #region Overrides
    public override void init()
    {
        base.init();        
        this.GetComponent<BoxCollider2D>().offset = this.sender.boxCollider.offset;
        this.GetComponent<BoxCollider2D>().size = this.sender.boxCollider.size;
    }

    #endregion
}
