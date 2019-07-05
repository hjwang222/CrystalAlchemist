using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class affectState : affectResource
{
    [FoldoutGroup("Change State", expanded: false)]
    [SerializeField]
    private CharacterState newCharacterState;

    #region Overrides

    public override void init()
    {
        base.init();
        this.target.currentState = this.newCharacterState;
    }

    #endregion
}
