using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;

public enum enumButton
{
    AButton,
    BButton,
    XButton,
    YButton,
    RBButton
}


public class Player : Character
{
    [BoxGroup("Pflichtfelder")]
    [SerializeField]
    private CastBar castbar;

    [BoxGroup("Pflichtfelder")]
    [Required]
    public TimeValue timeValue;

    [FoldoutGroup("Skills", expanded: false)]
    [Tooltip("Skills, welcher der Character verwenden kann")]
    public List<StandardSkill> skillSet = new List<StandardSkill>();

    [Required]
    [FoldoutGroup("Player Signals", expanded: false)]
    public StringSignal dialogBoxSignal;

    [Required]
    [FoldoutGroup("Player Signals", expanded: false)]
    public SimpleSignal healthSignalUI;

    [Required]
    [FoldoutGroup("Player Signals", expanded: false)]
    public SimpleSignal manaSignalUI;

    [Required]
    [FoldoutGroup("Player Signals", expanded: false)]
    public SimpleSignal currencySignalUI;

    [Required]
    [FoldoutGroup("Player Signals", expanded: false)]
    public SimpleSignal openInventorySignal;

    [Required]
    [FoldoutGroup("Player Signals", expanded: false)]
    public SimpleSignal openPauseSignal;

    [FoldoutGroup("Player Signals", expanded: false)]
    public BoolSignal cameraSignal;

    [FoldoutGroup("Player Signals", expanded: false)]
    public BoolSignal currencySignalUISound;

    [Required]
    [BoxGroup("Pflichtfelder")]
    [SerializeField]
    private GameObject targetHelpObject;

    [Required]
    [BoxGroup("Pflichtfelder")]
    [SerializeField]
    private BoolValue loadGame;

    [HideInInspector]
    public StandardSkill AButton;
    [HideInInspector]
    public StandardSkill BButton;
    [HideInInspector]
    public StandardSkill XButton;
    [HideInInspector]
    public StandardSkill YButton;
    [HideInInspector]
    public StandardSkill RBButton;

    private Vector3 change;
    private string lastButtonPressed = "";

    // Start is called before the first frame update
    private void Awake()
    {
       initPlayer();
    }

    private void initPlayer()
    {
        this.currencySignalUISound.Raise(false);
        SaveSystem.loadOptions();

        List<StandardSkill> tempSkillSet = new List<StandardSkill>();

        foreach (StandardSkill skill in this.skillSet)
        {
            tempSkillSet.Add(Utilities.Skill.setSkill(this, skill));
        }

        this.skillSet = tempSkillSet;

        this.isPlayer = true;
        this.init();
        this.setResourceSignal(this.healthSignalUI, this.manaSignalUI, this.currencySignalUI);

        if (this.loadGame.getValue()) LoadSystem.loadPlayerData(this);

        if (this.targetHelpObject != null) setTargetHelper(this.targetHelpObject);
        Utilities.Helper.checkIfHelperDeactivate(this);

        Utilities.UnityUtils.SetAnimatorParameter(this.animator, "moveX", 0);
        Utilities.UnityUtils.SetAnimatorParameter(this.animator, "moveY", -1);

        this.direction = new Vector2(0, -1);
        this.currencySignalUISound.Raise(true);
        //this.currencySignalUI.Raise();
    }

    public void loadSkillsFromSkillSet(string name, enumButton button)
    {
        foreach (StandardSkill skill in this.skillSet)
        {
            if (skill.skillName == name)
            {
                switch (button)
                {
                    case enumButton.AButton: this.AButton = skill; break;
                    case enumButton.BButton: this.BButton = skill; break;
                    case enumButton.XButton: this.XButton = skill; break;
                    case enumButton.YButton: this.YButton = skill; break;
                    case enumButton.RBButton: this.RBButton = skill; break;
                }

                break;
            }
        }
    }

    ///////////////////////////////////////////////////////////////

    // Update is called once per frame
    private new void Update()
    {
        base.Update();
        playerInputs();
    }

    private void playerInputs()
    {
        if (this.currentState == CharacterState.inDialog || this.currentState == CharacterState.inMenu)
        {
            Utilities.UnityUtils.SetAnimatorParameter(this.animator, "isWalking", false);
            return;
        }

        if (Input.GetButtonDown("Inventory"))
        {
            this.openInventorySignal.Raise();
        }

        if (Input.GetButtonDown("Pause"))
        {
            this.openPauseSignal.Raise();
        }

        if (!Utilities.StatusEffectUtil.isCharacterStunned(this))
        {
            change = Vector3.zero;
            change.x = Input.GetAxisRaw("Horizontal");
            change.y = Input.GetAxisRaw("Vertical");

            if (currentState != CharacterState.dead
                && this.currentState != CharacterState.inDialog
                && this.currentState != CharacterState.inMenu)
            {
                UpdateAnimationAndMove();
            }           
        }

        if (this.currentState != CharacterState.knockedback)
        {
            useSkill("A-Button");
            useSkill("B-Button");
            useSkill("X-Button");
            useSkill("Y-Button");
            useSkill("RB-Button");
        }
    }

    public void delay(CharacterState newState)
    {
        StartCoroutine(Utilities.Skill.delayInputPlayerCO(GlobalValues.playerDelay, this, newState));
    }

    public void showDialogBox(string text)
    {
        if (this.currentState != CharacterState.inDialog) this.dialogBoxSignal.Raise(text);
    }

    public string getScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        return scene.name;
    }

    ///////////////////////////////////////////////////////////////

    public void setNewPosition(Vector2 playerPositionInNewScene)
    {
        StartCoroutine(positionCo(playerPositionInNewScene));
    }

    private IEnumerator positionCo(Vector2 playerPositionInNewScene)
    {
        this.currentState = CharacterState.inDialog;
        this.enableSpriteRenderer(false);
        //this.cameraSignal.Raise(false);
        yield return null;

        this.transform.position = playerPositionInNewScene;
        this.enableSpriteRenderer(true);
        //this.cameraSignal.Raise(true);
        setPlayerPlayable();
    }

    public void setPlayerPlayable()
    {
        this.currentState = CharacterState.idle;
    }

    ///////////////////////////////////////////////////////////////


    #region Using Skill

    private StandardSkill getSkillFromButton(string button)
    {
        //TODO: GEHT BESSER!

        switch (button)
        {
            case "A-Button": return this.AButton;
            case "B-Button": return this.BButton;
            case "X-Button": return this.XButton;
            case "Y-Button": return this.YButton;
            case "RB-Button": return this.RBButton;
            default: return null;
        }
    }

    private void useSkill(string button)
    {
        StandardSkill skill = this.getSkillFromButton(button);

        if (skill != null)
        {
            if (skill.cooldownTimeLeft > 0)
            {
                skill.cooldownTimeLeft -= (Time.deltaTime * this.timeDistortion * this.spellspeed);
            }
            else if (this.currentState != CharacterState.interact
                 && this.currentState != CharacterState.inDialog
                 && this.currentState != CharacterState.inMenu
                 && !Utilities.StatusEffectUtil.isCharacterStunned(this))
            {
                int currentAmountOfSameSkills = Utilities.Skill.getAmountOfSameSkills(skill, this.activeSkills, this.activePets);

                if (currentAmountOfSameSkills < skill.maxAmounts
                        && (this.getResource(skill.resourceType, skill.item) + skill.addResourceSender >= 0
                        || skill.addResourceSender == -Utilities.maxFloatInfinite))
                {
                    if (isSkillReadyToUse(button, skill)) activateSkill(button, skill);
                    activateSkillFromTargetingSystem(skill);
                }
                else if (currentAmountOfSameSkills >= skill.maxAmounts
                     && ((skill.deactivateByButtonUp || skill.deactivateByButtonDown) || skill.delay == Utilities.maxFloatInfinite))
                {
                    deactivateSkill(button, skill);
                }
            }

            if (Utilities.StatusEffectUtil.isCharacterStunned(this))
            {
                if(!skill.keepHoldTimer) skill.holdTimer = 0;
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
                resetCast(skill);
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
                skill.showIndicator(); //Zeige Indikator beim Casten+
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
                hideCastBarAndIndicator(skill);
            }
            else if (skill.cast > 0 && this.activeCastbar != null && skill.holdTimer > 0)
            {
                this.activeCastbar.showCastBar();
            }
        }
        else if (Input.GetButtonUp(button))
        {
            setLastButtonPressed(button, skill);
            if (skill.speedDuringCasting != 0) this.updateSpeed(0);

            //Cast only
            if (skill.holdTimer >= skill.cast && skill.cast > 0)
            {
                return true;
            }
            
            resetCast(skill);
        }

        return false;
    }



    private void activateSkill(string button, StandardSkill skill)
    {
        hideCastBarAndIndicator(skill);

        if (skill.lockOn == null)
        {
            //Benutze Skill (ohne Zielerfassung)            
            skill.cooldownTimeLeft = skill.cooldown; //Reset cooldown
            if (!skill.isRapidFire) skill.holdTimer = 0;

            Utilities.Skill.instantiateSkill(skill, this);
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
            && this.activeLockOnTarget.GetComponent<TargetingSystem>().skillReadyForActivation
            && this.activeLockOnTarget.GetComponent<TargetingSystem>().skill == skill)
        {
            //Benutze Skill (mit Zielerfassung)   
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
                skill.cooldownTimeLeft = skill.cooldown; //Reset cooldown

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

    public void deactivateAllSkills()
    {
        for (int i = 0; i < this.activeSkills.Count; i++)
        {
            StandardSkill activeSkill = this.activeSkills[i];
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
        int i = 0;

        foreach (Character target in targetingSystem.sortedTargets)
        {
            if (target.currentState != CharacterState.dead && target.currentState != CharacterState.respawning)
            {
                bool playSoundEffect = false;
                if (i == 0 || skill.multiHitDelay > 0.3f) playSoundEffect = true;

                fireSkillToSingleTarget(target, damageReduce, playSoundEffect, skill);

                yield return new WaitForSeconds(skill.multiHitDelay);
            }
            i++;
        }

        Destroy(this.activeLockOnTarget);
        this.activeLockOnTarget = null;
    }

    private void fireSkillToSingleTarget(Character target, float damageReduce, bool playSoundeffect, StandardSkill skill)
    {
        StandardSkill temp = Utilities.Skill.instantiateSkill(skill, this, target, damageReduce);
        //Vermeidung, dass Audio zu stark abgespielt wird
        if (!playSoundeffect) temp.dontPlayAudio = true;
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

    ///////////////////////////////////////////////////////////////

    #region Movement

    private void UpdateAnimationAndMove()
    {
        if (change != Vector3.zero)
        {
            MoveCharacter();

            bool lockAnimation = false;

            foreach (StandardSkill skill in this.activeSkills)
            {
                if (skill.movementLocked)
                {
                    lockAnimation = true;
                    break;
                }
            }

            if (!lockAnimation)
            {
                this.direction = change;

                Utilities.UnityUtils.SetAnimatorParameter(this.animator, "moveX", change.x);
                Utilities.UnityUtils.SetAnimatorParameter(this.animator, "moveY", change.y);
            }

            Utilities.UnityUtils.SetAnimatorParameter(this.animator, "isWalking", true);
        }
        else
        {
            Utilities.UnityUtils.SetAnimatorParameter(this.animator, "isWalking", false);
            if (this.currentState == CharacterState.walk) this.currentState = CharacterState.idle;
        }
    }

    private void MoveCharacter()
    {
        if (this.currentState != CharacterState.knockedback
            && this.currentState != CharacterState.attack)
        {
            if(this.currentState != CharacterState.interact) this.currentState = CharacterState.walk;
            change.Normalize(); //Diagonal-Laufen fixen

            //this.myRigidbody.MovePosition(transform.position + change * this.speed * (Time.deltaTime * this.timeDistortion));
            //this.myRigidbody.velocity = Vector2.zero;

            Vector3 movement = new Vector3(change.x, change.y + (this.steps*this.change.x), 0.0f);
            if(!this.isOnIce) this.myRigidbody.velocity = (movement * speed * this.timeDistortion);            
        }

        //Debug.Log("Reset in Player Movement: " + this.myRigidbody.velocity);
        //this.myRigidbody.velocity = Vector2.zero;

        //Slide
        //Vector3 movement = new Vector3(change.x, change.y, 0.0f);
        //this.myRigidbody.AddForce(movement * speed);
    }

    #endregion


}
