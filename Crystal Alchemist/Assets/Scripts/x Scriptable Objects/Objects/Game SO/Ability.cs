using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public enum AbilityState
{
    disabled,
    onCooldown,
    notCharged,
    charged,
    targetRequired,
    lockOn,
    ready
}

public enum AbilityRequirements
{
    none,
    teleport
}

[CreateAssetMenu(menuName = "Game/Ability/Ability")]
public class Ability : ScriptableObject
{
    [BoxGroup("Objects")]
    [Required]
    public Skill skill;

    [BoxGroup("Objects")]
    [SerializeField]
    public LockOnSystem targetingSystem;

    [BoxGroup("Objects")]
    [SerializeField]
    public SkillBookInfo info;

    [BoxGroup("Restrictions")]
    [SerializeField]
    public float cooldown;

    [BoxGroup("Restrictions")]
    [ShowIf("isRapidFire")]
    [SerializeField]
    private float rapidFireDelay = 1;

    [BoxGroup("Restrictions")]
    public float castTime;

    [BoxGroup("Restrictions")]
    [SerializeField]
    private bool hasMaxAmount = false;

    [BoxGroup("Restrictions")]
    [ShowIf("hasMaxAmount")]
    [SerializeField]
    private int maxAmount = 1;

    [BoxGroup("Restrictions")]
    [SerializeField]
    public AbilityRequirements requirements = AbilityRequirements.none;

    [BoxGroup("Booleans")]
    [SerializeField]
    public bool isRapidFire = false;



    [BoxGroup("Booleans")]
    [SerializeField]
    public bool remoteActivation = false;

    [BoxGroup("Booleans")]
    [HideIf("isRapidFire")]
    [SerializeField]
    public bool deactivateButtonUp = false;

    [BoxGroup("Booleans")]
    [SerializeField]
    [HideIf("castTime", 0f)]
    public bool keepCast = false;

    [BoxGroup("Debug")]
    public float cooldownLeft;
    [BoxGroup("Debug")]
    public float holdTimer;
    [BoxGroup("Debug")]
    public AbilityState state;

    #region Update Functions

    public void Initialize()
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
        if (this.isRapidFire && this.deactivateButtonUp) this.deactivateButtonUp = false;

        this.cooldownLeft = 0;
        
        if (this.castTime > 0 && this.holdTimer < this.castTime) this.state = AbilityState.notCharged;
        else if (this.targetingSystem != null) this.state = AbilityState.targetRequired;
        else this.state = AbilityState.ready;
    }

    #endregion


    #region functions

    public void Charge()
    {
        if (this.holdTimer <= this.castTime)
        {
            this.holdTimer += Time.deltaTime;
            this.state = AbilityState.notCharged; //?
        }
        else
        {
            if (this.targetingSystem != null) this.state = AbilityState.targetRequired; //aufgeladen, aber Ziel benötigt!
            else this.state = AbilityState.charged; //aufgeladen!
        }
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

    public bool canUseAbility(Character character)
    {
        bool enoughResource = this.isResourceEnough(character);
        if (character.GetComponent<AI>()) enoughResource = true;

        bool notToMany = true;
        if (this.hasMaxAmount) notToMany = (getAmountOfSameSkills(this.skill, character.activeSkills, character.activePets) <= this.maxAmount);        

        return (notToMany && enoughResource);
    }

    private int getAmountOfSameSkills(Skill skill, List<Skill> activeSkills, List<Character> activePets)
    {
        int result = 0;
        SkillSummon summonSkill = skill.GetComponent<SkillSummon>();

        if (summonSkill == null)
        {
            for (int i = 0; i < activeSkills.Count; i++)
            {
                Skill activeSkill = activeSkills[i];
                if (activeSkill.skillName == skill.skillName) result++;
            }
        }
        else
        {
            for (int i = 0; i < activePets.Count; i++)
            {
                if (activePets[i] != null && activePets[i].stats.characterName == summonSkill.getPetName())
                {
                    result++;
                }
            }
        }

        return result;
    }

    private bool isResourceEnough(Character character)
    {
        SkillSenderModule senderModule = this.skill.GetComponent<SkillSenderModule>();

        if (senderModule == null) return true;
        else
        {
            if (character.getResource(senderModule.resourceType, senderModule.item) + senderModule.addResourceSender >= 0) return true;
            else return false;
        }
    }

    #endregion

}
