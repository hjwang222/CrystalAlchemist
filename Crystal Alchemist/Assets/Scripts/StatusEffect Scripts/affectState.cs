using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class affectState : affectResourceStatusEffect
{
    [FoldoutGroup("Change State", expanded: false)]
    [SerializeField]
    private CharacterState newCharacterState;

    [FoldoutGroup("Change State", expanded: false)]
    [SerializeField]
    private float speed;

    #region Overrides

    public override void init()
    {
        base.init();
        this.target.currentState = this.newCharacterState;
        this.target.updateSpeed(-88);
    }

    public override void DestroyIt()
    {
        //Zeit wieder normalisieren
        this.target.currentState = CharacterState.idle;
        this.target.updateSpeed(0);
        base.DestroyIt();
    }

    #endregion
}
