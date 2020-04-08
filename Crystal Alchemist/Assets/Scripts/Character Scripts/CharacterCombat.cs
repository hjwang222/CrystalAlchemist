using System.Collections.Generic;
using UnityEngine;

public class CharacterCombat : MonoBehaviour
{
    private CastBar activeCastBar;
    private TargetingSystem targetingSystem;

    public void InitializeTargeting(Character sender)
    {
        this.targetingSystem = Instantiate(GlobalGameObjects.targetingSystem, sender.transform.position, Quaternion.identity, sender.transform);
        this.targetingSystem.Initialize(sender);
        this.targetingSystem.name = GlobalGameObjects.targetingSystem.name;
        this.targetingSystem.gameObject.SetActive(false);
    }

    public void SetTimeValue(FloatValue timeValue)
    {
        this.targetingSystem.SetTimeValue(timeValue);
    }

    public void ChargeAbility(Ability ability, Character character)
    {
        ability.Charge(); //charge Skill when not full
        ShowCastBar(ability, character); //Show Castbar
        setSpeedDuringCasting(ability, character); //Set Speed during casting

        //Animations
    }

    public void UnChargeAbility(Ability ability, Character character)
    {
        ability.ResetCharge(); //reset charge when not full  
        HideCastBar(); //Hide Castbar
        deactivatePlayerButtonUp(ability, character); //deactivate Skill when button up, Player only
        resetSpeedAfterCasting(character); //set Speed to normal

        //Animations
    }

    private void setSpeedDuringCasting(Ability ability, Character character)
    {
        SkillSenderModule senderModule = ability.skill.GetComponent<SkillSenderModule>();
        if (senderModule != null) character.updateSpeed(senderModule.speedDuringCasting);
    }

    private void resetSpeedAfterCasting(Character character)
    {
        character.updateSpeed(0); //Set Speed to normal
    }

    private void deactivatePlayerButtonUp(Ability ability, Character character)
    {
        if (ability.state != AbilityState.onCooldown 
         && ability.deactivateButtonUp 
         && character.GetComponent<Player>() != null)
            deactivateSkill(ability, character);
    }

    private void deactivateSkill(Ability ability, Character character)
    {
        foreach (Skill skill in character.activeSkills)
        {
            if (skill.name == ability.skill.name)
            {
                skill.DeactivateIt();
                break;
            }
        }

        ability.ResetCoolDown();
    }


    public void ShowCastBar(Ability ability, Character character)
    {
        if (this.activeCastBar == null && ability.showCastbar && ability.hasCastTime)
        {
            this.activeCastBar = Instantiate(GlobalGameObjects.castBar, character.transform.position, Quaternion.identity);
            this.activeCastBar.setCastBar(character, ability);
        }
    }

    public void HideCastBar()
    {
        if (this.activeCastBar != null) this.activeCastBar.destroyIt();
    }


    public void showTargetingSystem(Ability ability)
    {
        if (!this.targetingSystem.gameObject.activeInHierarchy)
        {
            ability.state = AbilityState.lockOn;
            this.targetingSystem.setParameters(ability);
            this.targetingSystem.gameObject.SetActive(true);
        }
    }

    public void hideTargetingSystem(Ability ability, int count)
    {
        this.targetingSystem.gameObject.SetActive(false);

        if (count > 0) ability.ResetCoolDown(); //reset cooldown only when targets attacked
        else ability.state = AbilityState.targetRequired;
    }

    public float GetTargetingDelay()
    {
        return this.targetingSystem.getDelay();
    }

    public List<Character> GetTargetsFromTargeting()
    {
        return this.targetingSystem.getTargets();
    }
}
