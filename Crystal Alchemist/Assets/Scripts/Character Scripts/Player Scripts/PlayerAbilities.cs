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
    [Required]
    private FloatValue timeLeftValue;

    private void Start()
    {
        InitializeTargeting(this.player);
        this.SetTimeValue(this.timeLeftValue);
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
            if (ability.state == AbilityState.notCharged)
            {
                ChargeAbility(ability, this.player);
            }
            else if (ability.isRapidFire)
            {
                if (ability.state == AbilityState.charged) UseAbilityOnTarget(ability, this.player, null); //use rapidFire when charged
                else if (ability.state == AbilityState.ready) UseAbilityOnTarget(ability, this.player, null); //use rapidFire
                else if (ability.state == AbilityState.targetRequired) ShowTargetingSystem(ability); //show TargetingSystem
                else if (ability.state == AbilityState.lockOn) UseAbilityOnTargets(ability, this.player); //use TargetingSystem rapidfire  
            }
        }

        if (Input.GetButtonUp(button)) //release Button
        {
                 if (ability.state == AbilityState.charged && !ability.isRapidFire) UseAbilityOnTarget(ability, this.player, null); //use Skill when charged
            else if (ability.state == AbilityState.lockOn  &&  ability.isRapidFire) HideTargetingSystem(ability); //hide Targeting System when released

            UnChargeAbility(ability, this.player);
        }

        if (Input.GetButtonDown(button)) //press Button
        {
            if (ability.state == AbilityState.ready) UseAbilityOnTarget(ability, this.player, null); //use Skill
            else if (ability.state == AbilityState.targetRequired) ShowTargetingSystem(ability); //activate Targeting System
            else if (ability.state == AbilityState.lockOn)
            {                
                UseAbilityOnTargets(ability, this.player);//use Skill on locked Targets and hide Targeting System 
                HideTargetingSystem(ability);
            }
        }
    }
}
