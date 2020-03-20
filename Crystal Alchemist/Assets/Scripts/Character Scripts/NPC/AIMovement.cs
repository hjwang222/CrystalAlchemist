using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class AIMovement : MonoBehaviour
{
    [SerializeField]
    [Required]
    [BoxGroup("Required")]
    private AI npc;

    #region Parameter fürs Verfolgen
    [FoldoutGroup("Movement Attributes", expanded: false)]
    public bool prioritizePartner = false;

    [FoldoutGroup("Movement Attributes", expanded: false)]
    public float movementRadius = 0.1f;

    [FoldoutGroup("Movement Attributes", expanded: false)]
    public float movementRadiusPartner = 0.1f;

    [FoldoutGroup("Movement Attributes", expanded: false)]
    [SerializeField]
    private float movementDelay = 0;

    [FoldoutGroup("Movement Attributes", expanded: false)]
    [SerializeField]
    private bool backToStart = true;

    [FoldoutGroup("Movement Attributes", expanded: false)]
    [SerializeField]
    private List<Transform> path;

    [FoldoutGroup("Movement Attributes", expanded: false)]
    [ShowIf("path")]
    [SerializeField]
    private float followPathPrecision = 0.01f;

    [ShowIf("path")]
    [FoldoutGroup("Movement Attributes", expanded: false)]
    [SerializeField]
    private bool followPathInCircle = true;

    private bool standStill = false;
    private int factor = 1;
    private int currentPoint = 0;
    private Transform currentGoal;
    private bool startCoroutine = true;
    private bool activateSeeker = true;
    private Vector2 targetPosition = Vector2.zero;


    #endregion

    private void Start()
    {
        AnimatorUtil.SetAnimatorParameter(this.npc.animator, "isWalking", false);
    }

    #region Update und Movement Funktionen
    private void Update()
    {
        if (this.npc.currentState != CharacterState.knockedback && !this.npc.isOnIce)
        {
            if (this.npc.myRigidbody.bodyType != RigidbodyType2D.Static) this.npc.myRigidbody.velocity = Vector2.zero;
        }

        if (this.npc.currentState != CharacterState.dead
        && this.npc.currentState != CharacterState.knockedback
        && this.npc.currentState != CharacterState.manually) getTargetPosition();

        moveTorwardsTarget(this.targetPosition);
    }

    private void setTargetPath(Vector2 position)
    {
        if (this.targetPosition != position) this.targetPosition = position;
    }

    private void stopMoving()
    {
        this.targetPosition = Vector2.zero;
        AnimatorUtil.SetAnimatorParameter(this.npc.animator, "isWalking", false);
    }

    private bool movementPriority(bool priotizePartner)
    {
        if (priotizePartner)
        {
            //Partner > Target (loyal)
            if (this.npc.partner != null) moveToCharacter(this.npc.partner, this.movementRadiusPartner);
            else if (this.npc.target != null) moveToCharacter(this.npc.target, this.movementRadius);
            else return false;
        }
        else
        {
            //Target > Partner (aggressive)
            if (this.npc.target != null) moveToCharacter(this.npc.target, this.movementRadius);
            else if (this.npc.partner != null) moveToCharacter(this.npc.partner, this.movementRadiusPartner);
            else return false;
        }

        return true;
    }

    private void moveToCharacter(Character target, float radius)
    {
        if (target != null)
        {
            //Wenn der Spieler innerhalb vom Chase-Radius ist und auch nur solange bis der Gegner den Spieler nicht berührt!
            if (Vector3.Distance(target.transform.position, this.transform.position) > radius)
            {
                if (this.startCoroutine) StartCoroutine(delayMovementCo());

                if (!this.standStill)
                {
                    setTargetPath(target.transform.position);
                    //moveTorwardsTarget(target.transform.position);
                }
                else
                {
                    this.stopMoving();
                }
            }
            else
            {
                stopMoving();
                this.startCoroutine = true;
            }
        }
    }

    private void getTargetPosition()
    {
        //no target, no partner -> Patrol
        if (!movementPriority(this.prioritizePartner))
        {
            if (!this.standStill)
            {
                if (this.path.Count > 0)
                {
                    if (Vector3.Distance(transform.position, path[currentPoint].position) > followPathPrecision)
                    {
                        setTargetPath(path[currentPoint].position);
                        //moveTorwardsTarget(path[currentPoint].position);
                    }
                    else
                    {
                        stopMoving();

                        this.startCoroutine = true;
                        ChangeGoal();
                        StartCoroutine(delayMovementCo());
                    }
                }
                else if (this.backToStart && Vector2.Distance(this.npc.transform.position, this.npc.spawnPosition) > 0.1f) setTargetPath(this.npc.spawnPosition);
                else stopMoving();
            }
            else
            {
                stopMoving();
            }
            //Utilities.SetAnimatorParameter(this.animator, "isWakeUp", false);
        }

        this.npc.currentState = CharacterState.idle;
    }

    private IEnumerator delayMovementCo()
    {
        this.startCoroutine = false;
        this.standStill = true;
        yield return new WaitForSeconds(this.movementDelay);
        this.standStill = false;
    }

    private IEnumerator setNewPoint()
    {
        this.activateSeeker = false;
        yield return new WaitForSeconds(this.movementDelay);
        this.activateSeeker = true;
    }

    private void moveTorwardsTarget(Vector3 position)
    {
        if (this.npc.currentState != CharacterState.knockedback
            && this.npc.currentState != CharacterState.attack
            && this.npc.currentState != CharacterState.dead
            && position != Vector3.zero)
        {
            Vector2 direction = (position - this.transform.position).normalized;
            
            Vector3 movement = new Vector3(this.npc.direction.x, this.npc.direction.y + (this.npc.steps * this.npc.direction.x), 0.0f);
            if (!this.npc.isOnIce) this.npc.myRigidbody.velocity = (movement * this.npc.speed * this.npc.timeDistortion);           

            updateAnimation(direction);
        }
    }

    private void updateAnimation(Vector2 direction)
    {
        if (!StatusEffectUtil.isCharacterStunned(this.npc)) this.npc.changeAnim(direction.normalized);
        if (this.npc.flip) this.npc.setFlip();        

        this.npc.currentState = CharacterState.walk;
        AnimatorUtil.SetAnimatorParameter(this.npc.animator, "isWalking", true);
    }
    /*
    private void moveTorwardsTarget(Vector3 position)
    {
        if (this.npc.currentState != CharacterState.knockedback
            && this.npc.currentState != CharacterState.attack
            && this.npc.currentState != CharacterState.dead)
        {
            Vector2 direction = (position - this.transform.position).normalized;

            if (!Utilities.StatusEffectUtil.isCharacterStunned(this.npc)) this.npc.changeAnim(direction.normalized);

            if (this.npc.flip)
            {
                if (this.npc.direction.x < 0) this.npc.spriteRenderer.flipX = true;
                else this.npc.spriteRenderer.flipX = false;
            }

            Vector3 movement = new Vector3(this.npc.direction.x, this.npc.direction.y + (this.npc.steps * this.npc.direction.x), 0.0f);
            if (!this.npc.isOnIce) this.npc.myRigidbody.velocity = (movement * this.npc.speed * this.npc.timeDistortion);

            this.npc.currentState = CharacterState.walk;
            
            Utilities.UnityUtils.SetAnimatorParameter(this.npc.animator, "isWalking", true);
        }
    }*/

    private void ChangeGoal()
    {
        if (currentPoint == path.Count - 1) //letzter Knoten
        {
            if (this.followPathInCircle)
            {
                this.currentPoint = 0;
            }
            else
            {
                this.factor = -1; //rückwärts laufen
                currentPoint += this.factor;
            }
        }
        else if (currentPoint == 0)//erster Knoten 
        {
            if (!this.followPathInCircle)
            {
                this.factor = 1; //reset to normal
            }
            currentPoint += this.factor;
        }
        else
        {
            currentPoint += this.factor;
        }

        currentGoal = path[currentPoint];
    }

    #endregion

}
