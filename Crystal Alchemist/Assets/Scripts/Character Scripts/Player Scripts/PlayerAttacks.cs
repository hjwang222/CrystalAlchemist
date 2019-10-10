using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public enum enumButton
{
    AButton,
    BButton,
    XButton,
    YButton,
    RBButton
}

public class PlayerAttacks : MonoBehaviour
{
    [BoxGroup("Pflichtfelder")]
    [SerializeField]
    private CastBar castbar;

    [SerializeField]
    private Player player;
    
    [HideInInspector]
    public string currentButtonPressed = "";

    private void Awake()
    {
        List<StandardSkill> tempSkillSet = new List<StandardSkill>();

        foreach (StandardSkill skill in this.player.skillSet)
        {
            tempSkillSet.Add(Utilities.Skill.setSkill(this.player, skill));
        }

        this.player.skillSet = tempSkillSet;
    }

    public void loadSkillsFromSkillSet(string name, enumButton button)
    {
        foreach (StandardSkill skill in this.player.skillSet)
        {
            if (skill.skillName == name)
            {
                switch (button)
                {
                    case enumButton.AButton: this.player.AButton = skill; break;
                    case enumButton.BButton: this.player.BButton = skill; break;
                    case enumButton.XButton: this.player.XButton = skill; break;
                    case enumButton.YButton: this.player.YButton = skill; break;
                    case enumButton.RBButton: this.player.RBButton = skill; break;
                }

                break;
            }
        }
    }

    private StandardSkill getSkillFromButton(string button)
    {
        //TODO: GEHT BESSER!

        switch (button)
        {
            case "A-Button": return this.player.AButton;
            case "B-Button": return this.player.BButton;
            case "X-Button": return this.player.XButton;
            case "Y-Button": return this.player.YButton;
            case "RB-Button": return this.player.RBButton;
            default: return null;
        }
    }

    public void updateSkillButtons(string button)
    {
        StandardSkill skill = this.getSkillFromButton(button);

        if (skill != null)
        {
            if (skill.cooldownTimeLeft > 0)
            {
                skill.cooldownTimeLeft -= (Time.deltaTime * this.player.timeDistortion * this.player.spellspeed);
            }
            else if (this.player.currentState != CharacterState.interact
                 && this.player.currentState != CharacterState.inDialog
                 && this.player.currentState != CharacterState.respawning
                 && this.player.currentState != CharacterState.inMenu
                 && !Utilities.StatusEffectUtil.isCharacterStunned(this.player))
            {
                int currentAmountOfSameSkills = Utilities.Skill.getAmountOfSameSkills(skill, this.player.activeSkills, this.player.activePets);

                if (currentAmountOfSameSkills < skill.maxAmounts
                        && (this.player.getResource(skill.resourceType, skill.item) + skill.addResourceSender >= 0 //new method: Check if enough resource on skill
                        || skill.addResourceSender == -Utilities.maxFloatInfinite)
                        && skill.basicRequirementsExists)
                {
                    if (isSkillReadyToUse(button, skill))
                    {
                        activateSkill(button, skill); //activate Skill or Target System
                    }

                    activateSkillFromTargetingSystem(skill); //if Target System is ready
                }
                else if (currentAmountOfSameSkills >= skill.maxAmounts
                     && ((skill.deactivateByButtonUp || skill.deactivateByButtonDown) || skill.delay == Utilities.maxFloatInfinite))
                {
                    deactivateSkill(button, skill);
                }
            }

            if (Utilities.StatusEffectUtil.isCharacterStunned(this.player))
            {
                if (!skill.keepHoldTimer) skill.holdTimer = 0;
            }
        }
    }

    private bool isSkillReadyToUse(string button, StandardSkill skill)
    {
        if (isButtonUsable(button))
        {
            if (Input.GetButtonDown(button) && (skill.isRapidFire || skill.cast == 0))
            {
                setLastButtonPressed(button);

                if (skill.isRapidFire)
                {
                    this.player.resetCast(skill);
                }

                //Instants only (kein Cast und kein Rapidfire)
                if (skill.cast == 0) return true;
            }
            else if (Input.GetButton(button))
            {
                setLastButtonPressed(button);

                if (skill.speedDuringCasting != 0) this.player.updateSpeed(skill.speedDuringCasting);

                if (skill.holdTimer < skill.cast)
                {
                    skill.holdTimer += (Time.deltaTime * this.player.timeDistortion * this.player.spellspeed);
                    skill.showIndicator(); //Zeige Indikator beim Casten+
                    skill.showCastingAnimation();
                    skill.doOnCast();
                }

                if (skill.holdTimer >= skill.cast && skill.isRapidFire)
                {
                    //Rapidfire oder Cast Rapidfire                        
                    return true;
                }

                if (skill.cast > 0
                    && skill.holdTimer > 0
                    && skill.holdTimer < skill.cast
                    && this.player.activeCastbar == null
                    && this.castbar != null
                    && this.player.activeLockOnTarget == null)
                {
                    GameObject temp = Instantiate(this.castbar.gameObject, this.transform.position, Quaternion.identity, this.transform);
                    //temp.hideFlags = HideFlags.HideInHierarchy;
                    this.player.activeCastbar = temp.GetComponent<CastBar>();
                    this.player.activeCastbar.target = this.player;
                    this.player.activeCastbar.skill = skill;
                }
                else if (skill.cast > 0
                    && skill.holdTimer >= skill.cast
                    && this.player.activeCastbar != null
                    && skill.isRapidFire)
                {
                    this.player.hideCastBarAndIndicator(skill);
                }
                else if (skill.cast > 0 && this.player.activeCastbar != null && skill.holdTimer > 0)
                {
                    this.player.activeCastbar.showCastBar();
                    skill.showIndicator();
                    skill.showCastingAnimation();
                }
            }
            else if (Input.GetButtonUp(button))
            {
                setLastButtonPressed(button);
                if (skill.speedDuringCasting != 0) this.player.updateSpeed(0);

                //Cast only
                if (skill.holdTimer >= skill.cast && skill.cast > 0)
                {
                    return true;
                }

                this.player.resetCast(skill);
            }
        }

        return false;
    }

    private void activateSkill(string button, StandardSkill skill)
    {
        this.player.hideCastBarAndIndicator(skill);

        SkillTargetingSystemModule targetingSystemModule = skill.GetComponent<SkillTargetingSystemModule>();

        if (targetingSystemModule == null)
        {
            //Benutze Skill (ohne Zielerfassung)            
            skill.cooldownTimeLeft = skill.cooldown; //Reset cooldown
            if (!skill.isRapidFire) skill.holdTimer = 0;

            Utilities.Skill.instantiateSkill(skill, this.player);
        }
        else if (targetingSystemModule != null && targetingSystemModule.lockOn != null && this.player.activeLockOnTarget == null)
        {
            //Aktiviere Zielerfassung
            this.player.activeLockOnTarget = Instantiate(targetingSystemModule.lockOn, this.transform.position, Quaternion.identity, this.transform);
            TargetingSystem lockOnScript = this.player.activeLockOnTarget.GetComponent<TargetingSystem>();
            lockOnScript.button = button;
            lockOnScript.sender = this.player;
            lockOnScript.skill = skill;
            //this.activeLockOnTarget.hideFlags = HideFlags.HideInHierarchy; //TODO: Debug Value as Scriptable Object
        }
    }

    private void activateSkillFromTargetingSystem(StandardSkill skill)
    {
        if (this.player.activeLockOnTarget != null
            && this.player.activeLockOnTarget.GetComponent<TargetingSystem>().skillReadyForActivation
            && this.player.activeLockOnTarget.GetComponent<TargetingSystem>().skill == skill)
        {
            //Benutze Skill (mit Zielerfassung)   
            if (!skill.isRapidFire) skill.holdTimer = 0;

            TargetingSystem targetingSystem = this.player.activeLockOnTarget.GetComponent<TargetingSystem>();
            SkillTargetingSystemModule targetingSystemModule = skill.GetComponent<SkillTargetingSystemModule>();

            if (targetingSystem.currentTarget == null
                && targetingSystem.sortedTargets.Count == 0
                && targetingSystemModule.targetingMode != TargetingMode.autoMulti
                && targetingSystemModule.targetingMode != TargetingMode.autoSingle)
            {
                Destroy(this.player.activeLockOnTarget);
                this.player.activeLockOnTarget = null;
            }
            else if (targetingSystem.currentTarget != null
                || targetingSystem.sortedTargets.Count > 0
                || targetingSystemModule.targetingMode == TargetingMode.autoMulti
                || targetingSystemModule.targetingMode == TargetingMode.autoSingle)
            {
                skill.cooldownTimeLeft = skill.cooldown; //Reset cooldown

                if (targetingSystem.selectAll || targetingSystemModule.targetingMode == TargetingMode.autoMulti)
                {
                    for (int i = 0; i < targetingSystem.listOfTargetsWithMark.Count; i++)
                    {
                        Destroy(targetingSystem.listOfTargetsWithMark[i].gameObject);
                    }

                    //Multihit!
                    StartCoroutine(fireSkillToMultipleTargets(targetingSystem, skill));

                    //Fire Skill one time when no target is there
                    if (targetingSystemModule.targetingMode == TargetingMode.autoMulti && targetingSystem.sortedTargets.Count == 0)
                        fireSkillToSingleTarget(targetingSystem.currentTarget, 1, true, skill);
                }
                else if (!targetingSystem.selectAll || targetingSystemModule.targetingMode == TargetingMode.autoSingle)
                {
                    //SingleHit
                    Destroy(targetingSystem.singleTargetWithMark);
                    fireSkillToSingleTarget(targetingSystem.currentTarget, 1, true, skill);

                    Destroy(this.player.activeLockOnTarget);
                    this.player.activeLockOnTarget = null;
                }
            }
        }
    }

    public void deactivateAllSkills()
    {
        for (int i = 0; i < this.player.activeSkills.Count; i++)
        {
            StandardSkill activeSkill = this.player.activeSkills[i];
            activeSkill.durationTimeLeft = 0;
        }
    }

    private void deactivateSkill(string button, StandardSkill skill)
    {
        //Skill deaktivieren
        bool destroyit = false;

        if (Input.GetButtonUp(button) && skill.deactivateByButtonUp)
        {
            destroyit = true;
        }
        else if (Input.GetButtonDown(button) && skill.deactivateByButtonDown)
        {
            destroyit = true;
        }

        if (destroyit)
        {
            for (int i = 0; i < this.player.activeSkills.Count; i++)
            {
                StandardSkill activeSkill = this.player.activeSkills[i];
                if (activeSkill.skillName == skill.skillName)
                {
                    if (activeSkill.delay > 0) activeSkill.delayTimeLeft = 0; //C4
                    else activeSkill.durationTimeLeft = 0; //Schild
                }
            }
        }
    }

    private IEnumerator fireSkillToMultipleTargets(TargetingSystem targetingSystem, StandardSkill skill)
    {
        float damageReduce = targetingSystem.sortedTargets.Count;
        int i = 0;

        foreach (Character target in targetingSystem.sortedTargets)
        {
            if (target.currentState != CharacterState.dead
                && target.currentState != CharacterState.respawning)
            {
                bool playSoundEffect = false;
                if (i == 0 || skill.multiHitDelay > 0.3f) playSoundEffect = true;

                fireSkillToSingleTarget(target, damageReduce, playSoundEffect, skill);

                yield return new WaitForSeconds(skill.multiHitDelay);
            }
            i++;
        }

        Destroy(this.player.activeLockOnTarget);
        this.player.activeLockOnTarget = null;
    }

    private void fireSkillToSingleTarget(Character target, float damageReduce, bool playSoundeffect, StandardSkill skill)
    {
        StandardSkill temp = Utilities.Skill.instantiateSkill(skill, this.player, target, damageReduce);
        //Vermeidung, dass Audio zu stark abgespielt wird
        if (!playSoundeffect) temp.dontPlayAudio = true;
    }

    public bool isButtonUsable(string button)
    {
        if (button == this.currentButtonPressed
            || this.currentButtonPressed == null
            || this.currentButtonPressed == "") return true;
        else return false;
    }

    private void setLastButtonPressed(string button)
    {
        if (this.currentButtonPressed == "") this.currentButtonPressed = button;
        /*
        if (this.lastButtonPressed != button)
        {
            //if (!skill.keepHoldTimer) skill.holdTimer = 0;
            this.lastButtonPressed = button;
        }*/
    }

}
