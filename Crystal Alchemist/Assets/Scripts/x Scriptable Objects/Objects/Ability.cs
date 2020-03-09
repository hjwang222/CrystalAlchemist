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
    targeting,
    targetLocked,
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
    private int maxAmount = 1;

    [SerializeField]
    public bool isRapidFire = false;

    [SerializeField]
    public bool keepCast = false;

    [SerializeField]
    public LockOnSystem targetingSystem;

    public float cooldownLeft;
    public float holdTimer;

    public AbilityState state;
       

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
                this.state = AbilityState.targeting;
            }
            else this.state = AbilityState.ready;
        }
    }


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

    public void Use(Character sender)
    {
        Use(sender, null);
    }

    public void Use(Character sender, List<Character> targets)
    {
        this.cooldownLeft = this.cooldown;
        //Coroutine
    }




    private IEnumerator useSkill(Character sender, List<Character> targets, Skill skill)
    {
        if (targets != null && targets.Count > 0)
        {
            float damageReduce = targets.Count;
            int i = 0;

            foreach (Character target in targets)
            {
                if (target.currentState != CharacterState.dead
                    && target.currentState != CharacterState.respawning)
                {                    
                    CustomUtilities.Skills.instantiateSkill(skill, sender, target, damageReduce);
                    yield return new WaitForSeconds(1f);
                }
                i++;
            }
        }
        else
        {
            CustomUtilities.Skills.instantiateSkill(skill, sender);
        }
    }

    #endregion

}
