using Sirenix.OdinInspector;
using UnityEngine;

public class CharacterCombat : MonoBehaviour
{
    private CastBar activeCastBar;

    public void ChargeAbility(Ability ability, Character character)
    {
        ability.Charge(); //charge Skill when not full
        ShowCastBar(ability, character); //Show Castbar
        setSpeedDuringCasting(ability, character); //Set Speed during casting

        //Indicators
        //Animations
    }

    public void UnChargeAbility(Ability ability, Character character)
    {
        ability.ResetCharge(); //reset charge when not full  
        HideCastBar(); //Hide Castbar
        deactivatePlayerButtonUp(ability, character); //deactivate Skill when button up, Player only
        resetSpeedAfterCasting(character); //set Speed to normal

        //Indicators
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
        if (this.activeCastBar == null && ability.castTime > 0)
        {
            this.activeCastBar = Instantiate(GlobalGameObjects.castBar, character.transform.position, Quaternion.identity);
            this.activeCastBar.setCastBar(character, ability);
        }
    }

    public void HideCastBar()
    {
        if (this.activeCastBar != null) this.activeCastBar.destroyIt();
    }
}
