using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Character
{
    private Vector3 change;
    public ButtonConfig playerInputSkillConfig;
    private string lastButtonPressed = "";
    public GameObject music;

    private PlayerValues localPlayerData;

    public bool showDialog = false;
    public string dialogText = "";

    // Start is called before the first frame update
    private void Start()
    {        
        this.init();
        //SavePlayer();
        //LoadPlayer();
       
        this.currentState = CharacterState.walk;

        Utilities.SetParameter(this.animator, "moveX", 0);
        Utilities.SetParameter(this.animator, "moveY", -1);

        this.direction = new Vector2(0, -1);
    }
    /*
    private void LoadPlayer()
    {
        this.localPlayerData = GlobalPlayer.Instance.savedPlayerData;

        this.life = this.localPlayerData.life;
        this.attributeMaxLife = this.localPlayerData.maxLife;
        this.mana = this.localPlayerData.mana;
        this.attributeMaxMana = this.localPlayerData.maxMana;
        this.speed = this.localPlayerData.speed;
        this.spellspeed = this.localPlayerData.spellspeed;
        this.buffs = this.localPlayerData.buffs;
        this.debuffs = this.localPlayerData.debuffs;
    }

    public void SavePlayer()
    {
        this.localPlayerData = GlobalPlayer.Instance.savedPlayerData;
        this.localPlayerData.life = this.life;
        this.localPlayerData.maxLife = this.attributeMaxLife;
        this.localPlayerData.mana = this.mana;
        this.localPlayerData.maxMana = this.attributeMaxMana;
        this.localPlayerData.speed = this.speed;
        this.localPlayerData.spellspeed = this.spellspeed;
        this.localPlayerData.buffs = this.buffs;
        this.localPlayerData.debuffs = this.debuffs;

        GlobalPlayer.Instance.savedPlayerData = this.localPlayerData;
    }*/


    // Update is called once per frame
    private void Update()
    {       
        regeneration();

        if (currentState == CharacterState.inDialog)
        {
            Utilities.SetParameter(this.animator, "isWalking", false);
            return;
        }

        change = Vector3.zero;
        change.x = Input.GetAxisRaw("Horizontal");
        change.y = Input.GetAxisRaw("Vertical");

        if (this.currentState != CharacterState.attack
            && this.currentState != CharacterState.knockedback)
        {
            useSkill("A-Button");
            useSkill("B-Button");
            useSkill("X-Button");
            useSkill("Y-Button");
        }

        if (currentState == CharacterState.walk || this.currentState == CharacterState.idle || this.currentState == CharacterState.interact)
        {
            UpdateAnimationAndMove();
        }
    }


    public void showDialogBox(string text)
    {
        this.dialogText = text;
        this.showDialog = true;
    }

    

    #region Using Skill

    private void useSkill(string button)
    {
        if (this.currentState != CharacterState.interact && this.currentState != CharacterState.inDialog)
        {
            StandardSkill skill = this.playerInputSkillConfig.getSkillByButton(button);

            if (skill.cooldownTimeLeft > 0)
            {
                skill.cooldownTimeLeft -= (Time.deltaTime * this.timeDistortion * this.spellspeed);
            }
            else
            {
                int currentAmountOfSameSkills = getAmountOfSameSkills(skill);

                if (currentAmountOfSameSkills < skill.maxAmounts
                    && (this.mana + skill.addManaSender >= 0 || skill.addManaSender == -Utilities.maxFloatInfinite)
                    && this.life + skill.addLifeSender > 0)
                {
                    if (isSkillReadyToUse(button, skill)) activateSkill(button, skill);
                    activateSkillFromTargetingSystem(skill);
                }
                else if (currentAmountOfSameSkills >= skill.maxAmounts
                     && (skill.duration == Utilities.maxFloatInfinite || skill.delay == Utilities.maxFloatInfinite))
                {
                    deactivateSkill(button, skill);
                }
            }
        }
    }

    private bool isSkillReadyToUse(string button, StandardSkill skill)
    {
        if (Input.GetButtonDown(button) && (skill.isRapidFire || skill.cast == 0))
        {
            setLastButtonPressed(button, skill);

            if (skill.isRapidFire)
            {
                if (!skill.keepHoldTimer) skill.holdTimer = 0;
                if (this.activeCastbar != null)
                {
                    this.activeCastbar.destroyIt();
                }
            }

            //Instants only (kein Cast und kein Rapidfire)
            if (skill.cast == 0) return true;
        }
        else if (Input.GetButton(button))
        {
            setLastButtonPressed(button, skill);

            if (skill.speedDuringCasting != 0) updateSpeed(skill.speedDuringCasting);

            if (skill.holdTimer < skill.cast)
            {
                skill.holdTimer += (Time.deltaTime * this.timeDistortion * this.spellspeed);
            }

            if (skill.holdTimer >= skill.cast && skill.isRapidFire)
            {
                //Rapidfire oder Cast Rapidfire                        
                return true;
            }

            if (skill.cast > 0
                && skill.holdTimer > 0
                && skill.holdTimer < skill.cast
                && this.activeCastbar == null
                && this.castbar != null
                && this.activeLockOnTarget == null)
            {
                GameObject temp = Instantiate(this.castbar.gameObject, this.transform.position, Quaternion.identity, this.transform);
                //temp.hideFlags = HideFlags.HideInHierarchy;
                this.activeCastbar = temp.GetComponent<CastBar>();
                this.activeCastbar.target = this;
                this.activeCastbar.skill = skill;
            }
            else if (skill.cast > 0
                && skill.holdTimer >= skill.cast
                && this.activeCastbar != null
                && skill.isRapidFire)
            {
                this.activeCastbar.destroyIt();
            }
            else if (skill.cast > 0 && this.activeCastbar != null && skill.holdTimer > 0)
            {
                this.activeCastbar.showCastBar();
            }
        }
        else if (Input.GetButtonUp(button))
        {
            setLastButtonPressed(button, skill);

            //Cast only
            if (skill.holdTimer >= skill.cast && skill.cast > 0)
            {
                return true;
            }
            if (skill.speedDuringCasting != 0) this.updateSpeed(0);
            if (!skill.keepHoldTimer) skill.holdTimer = 0;
            if (this.activeCastbar != null) this.activeCastbar.destroyIt();
        }

        return false;
    }

    private void activateSkill(string button, StandardSkill skill)
    {
        if (this.activeCastbar != null) this.activeCastbar.destroyIt();

        if (skill.lockOn == null)
        {
            //Benutze Skill (ohne Zielerfassung)            
            skill.cooldownTimeLeft = skill.cooldown; //Reset cooldown
            if (!skill.isRapidFire) skill.holdTimer = 0;

            Utilities.fireSkill(skill, this);
        }
        else if (skill.lockOn != null && this.activeLockOnTarget == null)
        {
            //Aktiviere Zielerfassung
            this.activeLockOnTarget = Instantiate(skill.lockOn, this.transform.position, Quaternion.identity, this.transform);
            TargetingSystem lockOnScript = this.activeLockOnTarget.GetComponent<TargetingSystem>();
            lockOnScript.button = button;
            lockOnScript.sender = this;
            lockOnScript.skill = skill;
            //this.activeLockOnTarget.hideFlags = HideFlags.HideInHierarchy; //TODO: Debug Value as Scriptable Object
        }
    }

    private void activateSkillFromTargetingSystem(StandardSkill skill)
    {
        if (this.activeLockOnTarget != null
            && this.activeLockOnTarget.GetComponent<TargetingSystem>().targetsSet
            && this.activeLockOnTarget.GetComponent<TargetingSystem>().skill == skill)
        {
            //Benutze Skill (mit Zielerfassung)           
            skill.cooldownTimeLeft = skill.cooldown; //Reset cooldown
            if (!skill.isRapidFire) skill.holdTimer = 0;

            TargetingSystem targetingSystem = this.activeLockOnTarget.GetComponent<TargetingSystem>();

            if (targetingSystem.currentTarget == null
                && targetingSystem.sortedTargets.Count == 0
                && skill.targetingMode != TargetingMode.autoMulti
                && skill.targetingMode != TargetingMode.autoSingle)
            {
                Destroy(this.activeLockOnTarget);
                this.activeLockOnTarget = null;
            }
            else if (targetingSystem.currentTarget != null
                || targetingSystem.sortedTargets.Count > 0
                || skill.targetingMode == TargetingMode.autoMulti
                || skill.targetingMode == TargetingMode.autoSingle)
            {
                if (targetingSystem.selectAll || skill.targetingMode == TargetingMode.autoMulti)
                {
                    for (int i = 0; i < targetingSystem.listOfTargetsWithMark.Count; i++)
                    {
                        Destroy(targetingSystem.listOfTargetsWithMark[i].gameObject);
                    }

                    //Multihit!
                    StartCoroutine(fireSkillToMultipleTargets(targetingSystem, skill));

                    //Fire Skill one time when no target is there
                    if (skill.targetingMode == TargetingMode.autoMulti && targetingSystem.sortedTargets.Count == 0)
                        fireSkillToSingleTarget(targetingSystem.currentTarget, 1, true, skill);
                }
                else if (!targetingSystem.selectAll || skill.targetingMode == TargetingMode.autoSingle)
                {
                    //SingleHit
                    Destroy(targetingSystem.singleTargetWithMark);
                    fireSkillToSingleTarget(targetingSystem.currentTarget, 1, true, skill);

                    Destroy(this.activeLockOnTarget);
                    this.activeLockOnTarget = null;
                }
            }
        }
    }

    private void deactivateSkill(string button, StandardSkill skill)
    {
        //Skill deaktivieren
        bool destroyit = false;

        if (Input.GetButtonUp(button))
        {
            destroyit = true;
        }
        else if (Input.GetButtonDown(button))
        {
            destroyit = true;
        }

        if (destroyit)
        {
            for (int i = 0; i < this.activeSkills.Count; i++)
            {
                StandardSkill activeSkill = this.activeSkills[i];
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

        if (targetingSystem.sortedTargets.Count > 0 && targetingSystem.lastID == 0)
        {
            targetingSystem.lastID = targetingSystem.sortedTargets[targetingSystem.sortedTargets.Count - 1].gameObject.GetInstanceID();

            int ID = 1;

            for (int i = 0; ID != targetingSystem.lastID;)
            {
                if (targetingSystem.sortedTargets[i] == null) i++; //Springe weiter, wenn das Ziel nicht mehr existiert
                else
                {
                    Character target = targetingSystem.sortedTargets[i];
                    ID = target.gameObject.GetInstanceID();

                    if (targetingSystem.hittedIDs.Contains(ID))
                    {
                        i++; //Springe weiter, wenn das Ziel bereits getroffen wurde und noch existiert
                    }
                    else
                    {
                        bool playSoundEffect = false;
                        if (i == 0 || skill.multiHitDelay > 0.3f) playSoundEffect = true;

                        fireSkillToSingleTarget(target, damageReduce, playSoundEffect, skill);
                        targetingSystem.hittedIDs.Add(target.gameObject.GetInstanceID());
                        yield return new WaitForSeconds(skill.multiHitDelay);
                    }
                }
            }
        }

        Destroy(this.activeLockOnTarget);
        this.activeLockOnTarget = null;
    }

    private void fireSkillToSingleTarget(Character target, float damageReduce, bool playSoundeffect, StandardSkill skill)
    {
        StandardSkill temp = Utilities.instantiateSkill(skill, this, target, damageReduce);
        //Vermeidung, dass Audio zu stark abgespielt wird
        if (!playSoundeffect) temp.startSoundEffect = null;
    }

    private void setLastButtonPressed(string button, StandardSkill skill)
    {
        if (this.lastButtonPressed != button)
        {
            if (!skill.keepHoldTimer) skill.holdTimer = 0;
            this.lastButtonPressed = button;
        }
    }

    #endregion



    #region Movement

    private void UpdateAnimationAndMove()
    {
        if (change != Vector3.zero)
        {
            MoveCharacter();

            bool lockAnimation = false;

            foreach (StandardSkill skill in this.activeSkills)
            {
                if (skill.lockMovementonDuration)
                {
                    lockAnimation = true;
                    break;
                }
            }

            if (!lockAnimation)
            {
                this.direction = change;

                Utilities.SetParameter(this.animator, "moveX", change.x);
                Utilities.SetParameter(this.animator, "moveY", change.y);
            }

            
            Utilities.SetParameter(this.animator, "isWalking", true);
        }
        else Utilities.SetParameter(this.animator, "isWalking", false);
    }

    private void MoveCharacter()
    {
        change.Normalize(); //Diagonal-Laufen fixen
        this.myRigidbody.MovePosition(transform.position + change * this.speed * (Time.deltaTime * this.timeDistortion));
        this.myRigidbody.velocity = Vector2.zero;

        //Slide
        //Vector3 movement = new Vector3(change.x, change.y, 0.0f);
        //this.myRigidbody.AddForce(movement * speed);
    }

    #endregion

    
}
