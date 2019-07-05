using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class DashSkill : StandardSkill
{
    [FoldoutGroup("Dash", expanded: false)]
    [Tooltip("True = nach vorne, False = Knockback")]
    [SerializeField]
    private bool forward = false;

    [FoldoutGroup("Dash", expanded: false)]
    [Range(0, Utilities.maxFloatSmall)]
    [Tooltip("Stärke des Knockbacks")]
    [SerializeField]
    private float selfThrust = 4;

    [FoldoutGroup("Dash", expanded: false)]
    [Range(0, Utilities.maxFloatSmall)]
    [Tooltip("Dauer des Knockbacks")]
    [HideIf("selfThrust", 0f)]
    [SerializeField]
    private float selfThrustTime = 0.2f;


    #region Overrides
    public override void init()
    {
        base.init();

        this.duration = this.selfThrustTime;
        int direction = -1; //knockback
        if (forward) direction = 1; //dash

        Dash(direction);
    }
    #endregion


    #region Functions (private)
    public void Dash(int direction)
    {
        this.sender.knockBack(selfThrustTime, selfThrust, this.direction);
    }

    public override void DestroyIt()
    {        
        base.DestroyIt();
    }
    #endregion
}

