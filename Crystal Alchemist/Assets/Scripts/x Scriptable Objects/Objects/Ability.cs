using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AbilityState
{
    toMany,
    onCooldown,
    notCharged,
    charged,
    targetRequired,
    lockOn,
    ready
}


[CreateAssetMenu(menuName = "Game/Ability")]
public class Ability : ScriptableObject
{
    public Skill skill;

    [SerializeField]
    private float cooldown;

    public float castTime;

    [SerializeField]
    public int maxAmount = 1;

    [SerializeField]
    public bool isRapidFire = false;

    [SerializeField]
    public bool keepCast = false;

    [SerializeField]
    public LockOnSystem targetingSystem;

    public float cooldownLeft;
    public float holdTimer;

    public AbilityState state;

    #region Update Functions

    public void Update()
    {
        updateTimers();
    }

    private void updateTimers()
    {
        if (this.cooldownLeft > 0)
        {
            this.cooldownLeft -= Time.deltaTime;
            this.state = AbilityState.onCooldown;
        }
        else
        {
            this.cooldownLeft = 0;

            if (this.castTime > 0)
            {
                if (this.holdTimer <= this.castTime) this.state = AbilityState.notCharged;
                else this.state = AbilityState.charged;
            }
            else if (this.targetingSystem != null)
            {
                if(this.state != AbilityState.lockOn) this.state = AbilityState.targetRequired;
            }
            else this.state = AbilityState.ready;
        }
    }

    #endregion


    #region functions

    public void Charge()
    {
        if (this.holdTimer <= this.castTime) this.holdTimer += Time.deltaTime;        
    }

    public void ResetCharge()
    {
        if (!this.keepCast) this.holdTimer = 0;
        else if (this.keepCast && this.holdTimer > this.castTime) this.holdTimer = 0;
    }

    public void ResetCoolDown()
    {
        this.cooldownLeft = this.cooldown;
    }    

    #endregion

}
