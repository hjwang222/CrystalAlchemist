using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillMenuActiveSlots : MonoBehaviour
{
    private PlayerAbilities playerAbilities;

    [SerializeField]
    private PlayerStats playerStats;
    [SerializeField]
    private Image skillImage;
    [SerializeField]
    private enumButton button;
    [SerializeField]
    private SimpleSignal newAssignedSkillSignal;

    public Ability ability;

    void Start()
    {
        setImage();
    }

    private void OnEnable()
    {        
        setImage();
    }

    private void setImage()
    {
        if(this.playerAbilities == null) this.playerAbilities = this.playerStats.player.GetComponent<PlayerAbilities>();

        this.ability = this.playerAbilities.buttons.GetAbilityFromButton(this.button);
        
        if (this.ability != null)
        {
            this.skillImage.gameObject.SetActive(true);
            if(this.ability.info != null) this.skillImage.sprite = this.ability.info.icon;
        }
        else
        {
            this.skillImage.gameObject.SetActive(false);
        }
    }

    public void assignSkillToButton(SkillMenu skillMenu)
    {
        this.playerAbilities.buttons.SetAbilityToButton(skillMenu.selectedAbility, this.button);
        skillMenu.selectSkillFromSkillSet(null);
        //CustomUtilities.Helper.checkIfHelperDeactivate(this.playerAbilities);

        setImage();
        this.newAssignedSkillSignal.Raise();
    }


}
