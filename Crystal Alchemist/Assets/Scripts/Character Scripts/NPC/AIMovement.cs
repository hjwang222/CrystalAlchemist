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

    #endregion

    private void Start()
    {
        Utilities.UnityUtils.SetAnimatorParameter(this.npc.animator, "isWalking", false);
    }

    #region Update und Movement Funktionen
    private void Update()
    {
        if(this.npc.currentState != CharacterState.dead
        && this.npc.currentState != CharacterState.manually) moveCharacter();
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

                if (!this.standStill) moveTorwardsTarget(target.transform.position);
                else Utilities.UnityUtils.SetAnimatorParameter(this.npc.animator, "isWalking", false);
            }
            else
            {
                this.startCoroutine = true;
            }
        }
    }

    private void moveCharacter()
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
                        moveTorwardsTarget(path[currentPoint].position);
                    }
                    else
                    {
                        this.startCoroutine = true;
                        ChangeGoal();
                        StartCoroutine(delayMovementCo());
                    }
                }
                else if (this.backToStart && this.npc.transform.position != this.npc.spawnPosition) moveTorwardsTarget(this.npc.spawnPosition);
                else Utilities.UnityUtils.SetAnimatorParameter(this.npc.animator, "isWalking", false);
            }
            else
            {
                Utilities.UnityUtils.SetAnimatorParameter(this.npc.animator, "isWalking", false);
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

    private void moveTorwardsTarget(Vector3 position)
    {
        if ((this.npc.currentState == CharacterState.idle || this.npc.currentState == CharacterState.walk) && this.npc.currentState != CharacterState.knockedback)
        {
            //Bewegt den Gegner zum Spieler
            Vector3 temp = Vector3.MoveTowards(transform.position, position, this.npc.speed * (Time.deltaTime * this.npc.timeDistortion));

            Vector2 direction = position - this.transform.position;
            if (!Utilities.StatusEffectUtil.isCharacterStunned(this.npc)) this.npc.changeAnim(direction.normalized);

            if (this.npc.direction.x < 0) this.npc.spriteRenderer.flipX = true;
            else this.npc.spriteRenderer.flipX = false;

            this.npc.myRigidbody.MovePosition(temp);
            this.npc.myRigidbody.velocity = Vector2.zero;

            this.npc.currentState = CharacterState.walk;
            
            Utilities.UnityUtils.SetAnimatorParameter(this.npc.animator, "isWalking", true);
        }
    }

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
