using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class PlayerAbilities : MonoBehaviour
{
    //[SerializeField]
    private List<Ability> abilities = new List<Ability>();

    [SerializeField]
    [Required]
    private Player player;

    [SerializeField]
    private Ability AButton;

    [SerializeField]
    private CastBar castbar;

    [SerializeField]
    private TargetingSystem targetingSystem;

    public CastBar activeCastbar;
    public TargetingSystem activeTargetingSystem;

    private void Start()
    {
        //this.AButton = Instantiate(this.AButton);
    }

    private void Update()
    {
        this.AButton.Update();

        if (this.player.currentState != CharacterState.interact
                     && this.player.currentState != CharacterState.inDialog
                     && this.player.currentState != CharacterState.respawning
                     && this.player.currentState != CharacterState.inMenu
                     && !CustomUtilities.StatusEffectUtil.isCharacterStunned(this.player))
        {
            useSkill(this.AButton, "AButton");
        }
    }

    private void useSkill(Ability ability, string button)
    {        
        if(Input.GetButton(button)) //hold Button
        {
            if (ability.state == AbilityState.notCharged) ability.Charge(); //charge Skill when not full
            else if (ability.state == AbilityState.charged && ability.isRapidFire) ability.Use(this.player); //use rapidFire when charged
            else if (ability.state == AbilityState.ready && ability.isRapidFire) ability.Use(this.player); //use rapidFire     

            //Speed during casting
            showCastBar(ability);
        }
        if (Input.GetButtonUp(button)) //release Button
        {
            if (ability.state == AbilityState.charged && !ability.isRapidFire) ability.Use(this.player); //use Skill when charged

            //Speed during casting reset
            ability.ResetCharge(); //reset charge when not full
            hideCastBar();
        }
        else if (Input.GetButtonDown(button)) //press Button
        {
            if (ability.state == AbilityState.ready) ability.Use(this.player); //use Skill
            if (ability.state == AbilityState.targeting) showTargetingSystem(ability); //activate Targeting System
            if (ability.state == AbilityState.targetLocked) ability.Use(this.player, this.activeTargetingSystem.targets); //use Skill from Targeting System

            //hideTargetingSystem(); //hide targeting system
        }
    }














    private void showCastBar(Ability ability)
    {
        if (this.activeCastbar == null && ability.castTime > 0)
        {
            this.activeCastbar = Instantiate(this.castbar, this.player.transform.position, Quaternion.identity);
            this.activeCastbar.setCastBar(this.player, ability);
        }

        //Indicators
        //Animations
    }

    private void hideCastBar()
    {
        if (this.activeCastbar != null) this.activeCastbar.destroyIt();

        //Indicators
        //Animations
    }

    private void showTargetingSystem(Ability ability)
    {
        if (ability.targetingSystem != null && this.activeTargetingSystem == null)
        {
            this.activeTargetingSystem = Instantiate(this.targetingSystem, this.transform.position, Quaternion.identity, this.transform);
            this.activeTargetingSystem.setParameters(ability, this.player);
        }
    }

    private void hideTargetingSystem()
    {
        if (this.activeTargetingSystem != null) this.activeTargetingSystem.DestroyIt();
    }
}
