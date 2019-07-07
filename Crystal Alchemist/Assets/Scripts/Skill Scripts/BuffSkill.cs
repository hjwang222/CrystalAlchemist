using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class BuffSkill : StandardSkill
{
    [FoldoutGroup("Heal and Dispell", expanded: false)]
    [SerializeField]
    private bool dispell = false;

    [FoldoutGroup("Heal and Dispell", expanded: false)]
    [SerializeField]
    private Color targetColor;
    private Color tempColor;

    #region Overrides
    public override void init()
    {
        base.init();
        if (this.affectSelf) UseSkillOnSelf(); 
    }
    #endregion


    #region Functions (private)
    public void UseSkillOnSelf()
    {
        //Spieler selbst (Heals, Buffs)
        this.sender.gotHit(this);        

        if (dispell)
        {
            if (this.sender.debuffs.Count > 0) this.sender.RemoveStatusEffect(this.sender.debuffs[0], false);
        }

        if (this.targetColor != null)
        {
            this.sender.addColor(this.targetColor);
        }
    }

    public override void DestroyIt()
    {
        this.sender.resetColor(this.targetColor);
        base.DestroyIt();
    }
    #endregion
}

