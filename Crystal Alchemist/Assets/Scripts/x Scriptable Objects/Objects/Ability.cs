using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AbilityState
{
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
    [BoxGroup("Objects")]
    [Required]
    public Skill skill;

    [BoxGroup("Objects")]
    [SerializeField]
    public LockOnSystem targetingSystem;


    [BoxGroup("Restrictions")]
    [SerializeField]
    private float cooldown;

    [BoxGroup("Restrictions")]
    public float castTime;

    [BoxGroup("Restrictions")]
    [SerializeField]
    public int maxAmount = 1;


    [BoxGroup("Booleans")]
    [SerializeField]
    public bool isRapidFire = false;

    [BoxGroup("Booleans")]
    [SerializeField]
    public bool remoteActivation = false;

    [BoxGroup("Booleans")]
    [SerializeField]
    public bool deactivateButtonUp = false;

    [BoxGroup("Booleans")]
    [SerializeField]
    public bool keepCast = false;



    [BoxGroup("Debug")]
    public float cooldownLeft;
    [BoxGroup("Debug")]
    public float holdTimer;
    [BoxGroup("Debug")]
    public AbilityState state;

    #region Update Functions

    public void Start()
    {
        setStartParameters();
    }

    public void Update()
    {
        updateCooldown();
    }

    private void updateCooldown()
    {
        if (this.state == AbilityState.onCooldown)
        {
            if (this.cooldownLeft > 0) this.cooldownLeft -= Time.deltaTime;
            else setStartParameters();            
        }
    }

    private void setStartParameters()
    {
        this.cooldownLeft = 0;

        if (this.targetingSystem != null) this.state = AbilityState.targetRequired;
        else if (this.castTime > 0) this.state = AbilityState.notCharged;
        else this.state = AbilityState.ready;
    }

    #endregion


    #region functions

    public void Charge()
    {
        if (this.holdTimer <= this.castTime)
        {
            this.holdTimer += Time.deltaTime;
            this.state = AbilityState.notCharged;
        }
        else this.state = AbilityState.charged;
    }

    public void ResetCharge()
    {
        if (!this.keepCast) this.holdTimer = 0;
        else if (this.keepCast && this.holdTimer > this.castTime) this.holdTimer = 0;
    }

    public void ResetCoolDown()
    {
        this.cooldownLeft = this.cooldown;
        this.state = AbilityState.onCooldown;
    }

    #endregion

}
