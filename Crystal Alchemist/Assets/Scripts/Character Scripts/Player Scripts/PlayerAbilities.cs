using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class PlayerAbilities : CharacterCombat
{
    public PlayerSkillset skillSet;

    public PlayerButtons buttons;

    [SerializeField]
    [Required]
    private Player player;
           
    [SerializeField]
    private PlayerTargetingSystem targetingSystem;



    private void Start()
    {
        this.targetingSystem.gameObject.SetActive(false);
    }

    private void Update()
    {
        this.skillSet.Update();

        this.buttons.Updating(this.player);

        if (this.buttons.currentAbility != null 
         && this.buttons.currentAbility.enabled)
            useSkill(this.buttons.currentAbility, this.buttons.currentButton);        
    }

    private void useSkill(Ability ability, string button)
    {
        if (Input.GetButton(button)) //hold Button
        {
            if (ability.state == AbilityState.notCharged) ChargeAbility(ability, this.player);
            else if (ability.isRapidFire)
            {
                if (ability.state == AbilityState.charged) UseAbility(ability); //use rapidFire when charged
                else if (ability.state == AbilityState.ready) UseAbility(ability); //use rapidFire
                else if (ability.state == AbilityState.targetRequired) showTargetingSystem(ability); //show TargetingSystem
                else if (ability.state == AbilityState.lockOn) UseAbilityOnTargets(ability, false); //use TargetingSystem rapidfire
            }
        }
        if (Input.GetButtonUp(button)) //release Button
        {
            if (ability.state == AbilityState.charged && !ability.isRapidFire) UseAbility(ability); //use Skill when charged
            else if (ability.state == AbilityState.lockOn && ability.isRapidFire) hideTargetingSystem(ability, 1); //hide Targeting System when released

            UnChargeAbility(ability, this.player);
        }
        else if (Input.GetButtonDown(button)) //press Button
        {
            if (ability.state == AbilityState.ready) UseAbility(ability); //use Skill
            else if (ability.state == AbilityState.targetRequired) showTargetingSystem(ability); //activate Targeting System
            else if (ability.state == AbilityState.lockOn) UseAbilityOnTargets(ability, true);//use Skill on locked Targets and hide Targeting System
        }
    }  

    private void showTargetingSystem(Ability ability)
    {
        if (!this.targetingSystem.gameObject.activeInHierarchy)
        {
            ability.state = AbilityState.lockOn;
            this.targetingSystem.setParameters(ability);
            this.targetingSystem.gameObject.SetActive(true);
        }
    }


    #region useAbility

    private void UseAbility(Ability ability)
    {
        if (ability.canUseAbility(this.player))
        {
            AbilityUtil.instantiateSkill(ability.skill, this.player);
            if (!ability.deactivateButtonUp && !ability.remoteActivation) ability.ResetCoolDown();
        }
    }

    private void UseAbilityOnTargets(Ability ability, bool hideTargetingSystem)
    {
        List<Character> targets = new List<Character>();
        targets.AddRange(this.targetingSystem.getTargets());
        if (hideTargetingSystem) this.hideTargetingSystem(ability, targets.Count);

        if (ability.canUseAbility(this.player))
        {
            StartCoroutine(useSkill(ability, targets));
        }
    }

    private IEnumerator useSkill(Ability ability, List<Character> targets)
    {
        float damageReduce = targets.Count;
        int i = 0;

        foreach (Character target in targets)
        {
            if (target.currentState != CharacterState.dead
                && target.currentState != CharacterState.respawning)
            {
                AbilityUtil.instantiateSkill(ability.skill, this.player, target, damageReduce);
                yield return new WaitForSeconds(this.targetingSystem.getDelay());
            }
            i++;
        }
    }

    private void hideTargetingSystem(Ability ability, int count)
    {
        this.targetingSystem.gameObject.SetActive(false);

        if (count > 0) ability.ResetCoolDown(); //reset cooldown only when targets attacked
        else ability.state = AbilityState.targetRequired;
    }

    #endregion
    


}
