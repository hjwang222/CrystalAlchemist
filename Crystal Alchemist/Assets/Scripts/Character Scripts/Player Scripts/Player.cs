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
    public SimpleSignal deathSignal;

    [Required]
    [FoldoutGroup("Player Signals", expanded: false)]
    public SimpleSignal healthSignalUI;

    [Required]
    [FoldoutGroup("Player Signals", expanded: false)]
    public SimpleSignal manaSignalUI;

    [Required]
    [FoldoutGroup("Player Signals", expanded: false)]
    public SimpleSignal openInventorySignal;

    [Required]
    [FoldoutGroup("Player Signals", expanded: false)]
    public SimpleSignal openPauseSignal;

    [Required]
    [FoldoutGroup("Player Signals", expanded: false)]
    public BoolSignal fadeSignal;

    [Required]
    [BoxGroup("Pflichtfelder")]
    [SerializeField]
    private GameObject targetHelpObject;

    [Required]
    [BoxGroup("Pflichtfelder")]
    [SerializeField]
    private BoolValue loadGame;

    [Required]
    [BoxGroup("Pflichtfelder")]
    [SerializeField]
    private FloatValue fadingDuration;

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
    private string currentButtonPressed = "";

    private Vector2 lastSaveGamePosition;
    private string lastSaveGameScene;

    private Vector3 change;

    ///////////////////////////////////////////////////////////////

    private void Awake()
    {
       initPlayer();
    }

    public void initPlayer()
    {
        SaveSystem.loadOptions();

        List<StandardSkill> tempSkillSet = new List<StandardSkill>();

        foreach (StandardSkill skill in this.skillSet)
        {
            tempSkillSet.Add(Utilities.Skill.setSkill(this, skill));
        }

        this.skillSet = tempSkillSet;

        this.isPlayer = true;
        this.init();

        if (this.loadGame.getValue()) LoadSystem.loadPlayerData(this);

        if (this.targetHelpObject != null) setTargetHelper(this.targetHelpObject);
        Utilities.Helper.checkIfHelperDeactivate(this);

        Utilities.UnityUtils.SetAnimatorParameter(this.animator, "Dead", false);
        Utilities.UnityUtils.SetAnimatorParameter(this.animator, "moveX", 0);
        Utilities.UnityUtils.SetAnimatorParameter(this.animator, "moveY", -1);

        this.direction = new Vector2(0, -1);
        //this.currencySignalUI.Raise();
    }


    private new void Update()
    {
        base.Update();
        playerInputs();
    }

    private void playerInputs()
    {
        if (this.currentState != CharacterState.dead)
        {
            if (this.currentState == CharacterState.inDialog || this.currentState == CharacterState.inMenu || this.currentState == CharacterState.respawning)
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
                    && this.currentState != CharacterState.inMenu
                    && this.currentState != CharacterState.respawning)
                {
                    UpdateAnimationAndMove();
                }
            }

            if (this.currentState != CharacterState.knockedback)
            {
                if (!isButtonPressed("A-Button")
                && !isButtonPressed("B-Button")
                && !isButtonPressed("X-Button")
                && !isButtonPressed("Y-Button")
                && !isButtonPressed("RB-Button")) this.currentButtonPressed = "";

                updateSkillButtons("A-Button");
                updateSkillButtons("B-Button");
                updateSkillButtons("X-Button");
                updateSkillButtons("Y-Button");
                updateSkillButtons("RB-Button");
            }
        }
    }

    private bool isButtonPressed(string button)
    {
        if (Input.GetButton(button)
            || Input.GetButtonUp(button)
            || Input.GetButtonDown(button)) return true;
        else return false;
    }

    public void delay(CharacterState newState)
    {
        StartCoroutine(Utilities.Skill.delayInputPlayerCO(GlobalValues.playerDelay, this, newState));
    }

    public void showDialogBox(string text)
    {
        if (this.currentState != CharacterState.inDialog) this.dialogBoxSignal.Raise(text);
    }

    public override void KillIt()
    {
        if (this.currentState != CharacterState.dead)
        {
            this.change = Vector2.zero;
            this.direction = new Vector2(0, -1);

            //TODO: Kill sofort (Skill noch aktiv)
            Utilities.StatusEffectUtil.RemoveAllStatusEffects(this.debuffs);
            Utilities.StatusEffectUtil.RemoveAllStatusEffects(this.buffs);

            this.spriteRenderer.color = Color.white;

            Utilities.UnityUtils.SetAnimatorParameter(this.animator, "moveX", 0);
            Utilities.UnityUtils.SetAnimatorParameter(this.animator, "moveY", -1);
            Utilities.UnityUtils.SetAnimatorParameter(this.animator, "Dead", true);

            this.currentState = CharacterState.dead;
            this.deathSignal.Raise();
        }
    }

    ///////////////////////////////////////////////////////////////


    public void setLastTeleport(string targetScene, Vector2 position)
    {
        this.lastSaveGamePosition = position;
        this.lastSaveGameScene = targetScene;
    }

    public bool getLastTeleport()
    {
        return getLastTeleport(out string scene, out Vector2 position);
    }

    public bool getLastTeleport(out string scene, out Vector2 position)
    {
        scene = this.lastSaveGameScene;
        position = this.lastSaveGamePosition;

        if (scene != null && position != null) return true;
        else return false;
    }

    public void teleportPlayer(string targetScene, Vector2 position, bool showAnimation)
    {
        StartCoroutine(LoadScene(targetScene, position, this.fadingDuration.getValue(), showAnimation));
    }

    public void teleportPlayer(string targetScene, Vector2 position, float duration, bool showAnimation)
    {
        StartCoroutine(LoadScene(targetScene, position, duration, showAnimation));
    }

    private IEnumerator LoadScene(string targetScene, Vector2 position, float duration, bool showAnimation)
    {
        this.currentState = CharacterState.respawning;
        this.deactivateAllSkills();

        if (showAnimation && this.respawnAnimation != null)
        {
            RespawnAnimation respawnObject = Instantiate(this.respawnAnimation, this.transform.position, Quaternion.identity);
            respawnObject.setCharacter(this, true);
            yield return new WaitForSeconds(respawnObject.getAnimationLength());
            this.enableSpriteRenderer(false);
            //yield return new WaitForSeconds(2f);
        }
        else
        {
            this.enableSpriteRenderer(false);
        }

        this.fadeSignal.Raise(false);

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(targetScene);
        asyncOperation.allowSceneActivation = false;

        while (!asyncOperation.isDone)
        {
            if (asyncOperation.progress >= 0.9f)
            {
                yield return new WaitForSeconds(duration);

                asyncOperation.allowSceneActivation = true;
                StartCoroutine(positionCo(position, showAnimation));
            }
            yield return null;
        }
    }

    private IEnumerator positionCo(Vector2 playerPositionInNewScene, bool showAnimation)
    {
        this.transform.position = playerPositionInNewScene;        

        if (showAnimation && this.respawnAnimation != null)
        {
            yield return new WaitForSeconds(2f);

            RespawnAnimation respawnObject = Instantiate(this.respawnAnimation, playerPositionInNewScene, Quaternion.identity);
            respawnObject.setCharacter(this);
            yield return new WaitForSeconds(respawnObject.getAnimationLength());

            yield return new WaitForSeconds(1f);
        }

        //this.transform.position = playerPositionInNewScene;
        this.enableSpriteRenderer(true);
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

    private void updateSkillButtons(string button)
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
                 && this.currentState != CharacterState.respawning
                 && this.currentState != CharacterState.inMenu
                 && !Utilities.StatusEffectUtil.isCharacterStunned(this))
            {
                int currentAmountOfSameSkills = Utilities.Skill.getAmountOfSameSkills(skill, this.activeSkills, this.activePets);

                if (currentAmountOfSameSkills < skill.maxAmounts
                        && (this.getResource(skill.resourceType, skill.item) + skill.addResourceSender >= 0
                        || skill.addResourceSender == -Utilities.maxFloatInfinite))
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

            if (Utilities.StatusEffectUtil.isCharacterStunned(this))
            {
                if(!skill.keepHoldTimer) skill.holdTimer = 0;
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
                    resetCast(skill);
                }

                //Instants only (kein Cast und kein Rapidfire)
                if (skill.cast == 0) return true;
            }
            else if (Input.GetButton(button))
            {
                setLastButtonPressed(button);

                if (skill.speedDuringCasting != 0) updateSpeed(skill.speedDuringCasting);

                if (skill.holdTimer < skill.cast)
                {
                    skill.holdTimer += (Time.deltaTime * this.timeDistortion * this.spellspeed);
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
                    skill.showIndicator();
                    skill.showCastingAnimation();
                }
            }
            else if (Input.GetButtonUp(button))
            {
                setLastButtonPressed(button);
                if (skill.speedDuringCasting != 0) this.updateSpeed(0);

                //Cast only
                if (skill.holdTimer >= skill.cast && skill.cast > 0)
                {
                    return true;
                }

                resetCast(skill);
            }
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

        Destroy(this.activeLockOnTarget);
        this.activeLockOnTarget = null;
    }

    private void fireSkillToSingleTarget(Character target, float damageReduce, bool playSoundeffect, StandardSkill skill)
    {
        StandardSkill temp = Utilities.Skill.instantiateSkill(skill, this, target, damageReduce);
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
        if(this.currentButtonPressed == "") this.currentButtonPressed = button;
        /*
        if (this.lastButtonPressed != button)
        {
            //if (!skill.keepHoldTimer) skill.holdTimer = 0;
            this.lastButtonPressed = button;
        }*/
    }

    public override void updateResource(ResourceType type, Item item, float value, bool showingDamageNumber)
    {
        base.updateResource(type, item, value, showingDamageNumber);

        switch (type)
        {
            case ResourceType.life:
                {
                    callSignal(this.healthSignalUI, value); break;
                }
            case ResourceType.mana:
                {
                    callSignal(this.manaSignalUI, value); break;
                }
        }
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
            && this.currentState != CharacterState.attack
            && this.currentState != CharacterState.dead)
        {
            if(this.currentState != CharacterState.interact) this.currentState = CharacterState.walk;
            change.Normalize(); //Diagonal-Laufen fixen

            //this.myRigidbody.MovePosition(transform.position + change * this.speed * (Time.deltaTime * this.timeDistortion));
            //this.myRigidbody.velocity = Vector2.zero;

            Vector3 movement = new Vector3(change.x, change.y + (this.steps*this.change.x), 0.0f);
            if(!this.isOnIce) this.myRigidbody.velocity = (movement * speed * this.timeDistortion);            
        }
    }

    #endregion


}
