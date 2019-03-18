using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : Character
{
    private Vector3 change;
    public AudioClip attackClip;
    public ButtonConfig playerInputSkillConfig;
    private string lastButtonPressed = "";
    public GameObject music;


    // Start is called before the first frame update
    void Start()
    {
        this.init();
        healthSignal.Raise();
        manaSignal.Raise();
        this.currentState = CharacterState.walk;
        
        animator.SetFloat("moveX", 0);
        animator.SetFloat("moveY", -1);

        this.direction = new Vector2(0, -1);
    }


    // Update is called once per frame
    void Update()
    {
        //resetVelocity();
        regeneration();

        if (currentState == CharacterState.inDialog)
        {
            animator.SetBool("isWalking", false);
            return;
        }

        change = Vector3.zero;
        change.x = Input.GetAxisRaw("Horizontal");
        change.y = Input.GetAxisRaw("Vertical");

        if (this.currentState != CharacterState.attack
            && this.currentState != CharacterState.knockedback)
        {
            
            //if (Input.GetButtonDown("A-Button"))            
                //AButtonPressed();     

            


            useSkill("A-Button");
            useSkill("B-Button");
            useSkill("X-Button");
            useSkill("Y-Button");
        }

        if (currentState == CharacterState.walk || this.currentState == CharacterState.idle || this.currentState == CharacterState.interact)
        {
            UpdateAnimationAndMove();
        }

        if (Input.GetKeyDown("escape"))
        {
            Debug.Log("EXIT");
            Application.Quit();
        }
    }





    public void useSkill(string button)
    {
        if (this.currentState != CharacterState.interact && this.currentState != CharacterState.inDialog)
        {
            Skill skill = this.playerInputSkillConfig.getSkillByButton(button);            
            
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

    private bool isSkillReadyToUse(string button, Skill skill)
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
                temp.hideFlags = HideFlags.HideInHierarchy;
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

    private void activateSkill(string button, Skill skill)
    {
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
            this.activeLockOnTarget.hideFlags = HideFlags.HideInHierarchy; //TODO: Debug Value as Scriptable Object
        }       
    }

    private void activateSkillFromTargetingSystem(Skill skill)
    {
        if (this.activeLockOnTarget != null
            && this.activeLockOnTarget.GetComponent<TargetingSystem>().targetsSet
            && this.activeLockOnTarget.GetComponent<TargetingSystem>().skill == skill)
        {
            //Benutze Skill (mit Zielerfassung)           
            skill.cooldownTimeLeft = skill.cooldown; //Reset cooldown
            if (!skill.isRapidFire) skill.holdTimer = 0;

            TargetingSystem targetingSystem = this.activeLockOnTarget.GetComponent<TargetingSystem>();

            if (targetingSystem.currentTarget == null && targetingSystem.sortedTargets.Count == 0)
            {                
                Destroy(this.activeLockOnTarget);
                this.activeLockOnTarget = null;
            }
            else
            {
                if (targetingSystem.selectAll)
                {
                    for (int i = 0; i < targetingSystem.listOfTargetsWithMark.Count; i++)
                    {
                        Destroy(targetingSystem.listOfTargetsWithMark[i].gameObject);
                    }

                    //Multihit!
                    StartCoroutine(fireSkillToTarget(targetingSystem, skill));
                }
                else
                {
                    //SingleHit
                    Destroy(targetingSystem.singleTargetWithMark);
                    fireSkillToTarget(targetingSystem.currentTarget, 1, true, skill);
                                        
                    Destroy(this.activeLockOnTarget);
                    this.activeLockOnTarget = null;
                }
            }
        }
    }

    private void deactivateSkill(string button, Skill skill)
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
                Skill activeSkill = this.activeSkills[i];
                if (activeSkill.skillName == skill.skillName)
                {
                    if (activeSkill.delay > 0) activeSkill.delayTimeLeft = 0; //C4
                    else activeSkill.durationTimeLeft = 0; //Schild
                }
            }
        }
    }

    private IEnumerator fireSkillToTarget(TargetingSystem targetingSystem, Skill skill)
    {
        float damageReduce = targetingSystem.sortedTargets.Count;

        for (int i = 0; i < targetingSystem.sortedTargets.Count; i++)
        {
            bool playSoundEffect = false;
            if (i == 0 || skill.multiHitDelay > 0.3f) playSoundEffect = true;

            Character target = targetingSystem.sortedTargets[i];
            fireSkillToTarget(target, damageReduce, playSoundEffect, skill);

            yield return new WaitForSeconds(skill.multiHitDelay);
        }
       
        Destroy(this.activeLockOnTarget);
        this.activeLockOnTarget = null;
    }

    private void fireSkillToTarget(Character target, float damageReduce, bool playSoundeffect, Skill skill)
    {
        Skill temp = Utilities.instantiateSkill(skill, this, target, damageReduce);
        //Vermeidung, dass Audio zu stark abgespielt wird
        if (!playSoundeffect) temp.startSoundEffect = null;
    }

    private void setLastButtonPressed(string button, Skill skill)
    {
        if (this.lastButtonPressed != button)
        {
            if (!skill.keepHoldTimer) skill.holdTimer = 0;
            this.lastButtonPressed = button;
        }
    }




    #region OLD STUFF

    public void AButtonPressed()
    {
        StartCoroutine(AttackCo());
    }

    private IEnumerator AttackCo()
    {
        if (currentState != CharacterState.interact)
        {
            Utilities.playSoundEffect(this.audioSource, this.attackClip, this.soundEffectVolume);
            animator.SetBool("isAttacking", true);
            this.currentState = CharacterState.attack;
            yield return null;
        }
        animator.SetBool("isAttacking", false);
        yield return new WaitForSeconds(0.3f);

        if (currentState != CharacterState.inDialog)
        {
            this.currentState = CharacterState.walk;
        }
    }

    #endregion

    private void UpdateAnimationAndMove()
    {
        if (change != Vector3.zero)
        {
            MoveCharacter();
            
            bool lockAnimation = false;

            foreach (Skill skill in this.activeSkills)
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
                animator.SetFloat("moveX", change.x);
                animator.SetFloat("moveY", change.y);
            }

            animator.SetBool("isWalking", true);
        }
        else animator.SetBool("isWalking", false);
    }

    void MoveCharacter()
    {
        change.Normalize(); //Diagonal-Laufen fixen
        this.myRigidbody.MovePosition(transform.position + change * this.speed * (Time.deltaTime*this.timeDistortion));
        this.myRigidbody.velocity = Vector2.zero;

        //Slide
        //Vector3 movement = new Vector3(change.x, change.y, 0.0f);
        //this.myRigidbody.AddForce(movement * speed);
    }

}
