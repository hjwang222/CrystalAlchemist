using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

public enum MovementPriority
{
    target,
    partner
}

public class AIMovement : MonoBehaviour
{
    [SerializeField]
    [Required]
    [BoxGroup("Required")]
    private AI npc;

    #region Parameter fürs Verfolgen
    [BoxGroup("Character Movement")]
    [SerializeField]
    private MovementPriority movementPriority = MovementPriority.target;

    [BoxGroup("Character Movement")]
    [SerializeField]
    private float targetRadius = 0.1f;

    [BoxGroup("Character Movement")]
    [SerializeField]
    private float partnerRadius = 0.1f;

    [BoxGroup("Character Movement")]
    [SerializeField]
    private float delay = 0;



    [BoxGroup("Movement Attributes")]
    [SerializeField]
    private bool backToStart = true;

    [ShowIf("backToStart")]
    [BoxGroup("Movement Attributes")]
    [SerializeField]
    private float returnDelay = 3f;



    [BoxGroup("Patrol")]
    [SerializeField]
    private bool isPatrol = false;

    [ShowIf("isPatrol")]
    [BoxGroup("Patrol")]
    [SerializeField]
    private float patrolDelay = 3f;

    [ShowIf("isPatrol")]
    [BoxGroup("Patrol")]
    [SerializeField]
    private List<Transform> patrolPath = new List<Transform>();

    [ShowIf("isPatrol")]
    [BoxGroup("Patrol")]
    [SerializeField]
    private float followPathPrecision = 0.01f;

    [ShowIf("isPatrol")]
    [BoxGroup("Patrol")]
    [SerializeField]
    private bool followPathInCircle = true;


    [BoxGroup("Pathfinding")]
    [SerializeField]
    private PathSeeker seeker;

    [BoxGroup("Pathfinding")]
    [SerializeField]
    [ShowIf("seeker")]
    private float accuracy = 0.25f;

    private List<Vector2> path;
    private int index;
    private bool wait = false;
    private bool enableCoroutine = true;
    private int factor = 1;
    private int currentPoint = 0;
    private Vector3 targetPosition;

    #endregion

    private void Start()
    {
        AnimatorUtil.SetAnimatorParameter(this.npc.animator, "isWalking", false);
        this.targetPosition = GlobalGameObjects.staticValues.nullVector;
    }

    #region Update und Movement Funktionen
    private void Update()
    {
        if (this.npc.currentState != CharacterState.knockedback && !this.npc.isOnIce)
        {
            if (this.npc.myRigidbody.bodyType != RigidbodyType2D.Static) this.npc.myRigidbody.velocity = Vector2.zero;
        }

        if (!this.wait)
        {
            UpdateTargetPosition();
            if (this.seeker != null) MoveTroughPaths(this.targetPosition); //Pathfinding
            else MoveToPosition(this.targetPosition); //No Pathfinding
        }
    }

    private void UpdateTargetPosition()
    {
        if (this.npc.currentState != CharacterState.dead
        && this.npc.currentState != CharacterState.knockedback
        && this.npc.currentState != CharacterState.manually)
        {
            SetNextPoint();
            this.npc.currentState = CharacterState.idle;
        }
    }

    private void SetNextPoint()
    {
        Vector3 patrolVector = GlobalGameObjects.staticValues.nullVector;
        Vector3 spawnVector = GlobalGameObjects.staticValues.nullVector;
        Vector3 chaseVector = GlobalGameObjects.staticValues.nullVector;

        if (this.isPatrol) patrolVector = GetNextPoint(patrolPath[currentPoint].position, this.followPathPrecision, SetNextWayPoint);
        if (this.backToStart) spawnVector = GetNextPoint(this.npc.spawnPosition, 0.25f);

        if (this.movementPriority == MovementPriority.partner)
        {
            chaseVector = CheckTargets(this.npc.partner, this.partnerRadius, this.npc.target, this.targetRadius);
        }
        else
        {
            chaseVector = CheckTargets(this.npc.target, this.targetRadius, this.npc.partner, this.partnerRadius);
        }

        SetVector(patrolVector, spawnVector, chaseVector);
    }

    private void SetVector(Vector3 patrolVector, Vector3 spawnVector, Vector3 chaseVector)
    {
        Vector3 newVector = GlobalGameObjects.staticValues.nullVector;
        float delay = 0f;

        SetVector(patrolVector, this.patrolDelay, newVector, delay, out newVector, out delay);
        SetVector(spawnVector, this.returnDelay, newVector, delay, out newVector, out delay);
        SetVector(chaseVector, this.delay, newVector, delay, out newVector, out delay);

        if (newVector != this.targetPosition)
        {
            targetPosition = newVector;
            if (this.enableCoroutine) StartCoroutine(delayMovementCo(delay));
        }
    }

    private void SetVector(Vector3 vector, float delay, Vector3 _newVector, float _delay, out Vector3 newVector, out float newDelay)
    {
        newDelay = _delay;
        newVector = _newVector;

        if (vector != GlobalGameObjects.staticValues.nullVector)
        {
            newVector = vector;
            newDelay = delay;
        }
    }

    #region Check Distances

    private Vector3 CheckTargets(Character first, float firstRadius, Character second, float secondRadius)
    {
        if (first != null) return GetNextPoint(first.GetGroundPosition(), firstRadius);
        else if(second != null) return GetNextPoint(second.GetGroundPosition(), secondRadius);        
        else
        {
            this.enableCoroutine = true;
            return GlobalGameObjects.staticValues.nullVector;
        }
    }

    private Vector3 GetNextPoint(Vector2 nextPosition, float maxDistance)
    {
        return GetNextPoint(nextPosition, maxDistance, null);
    }

    private Vector3 GetNextPoint(Vector2 nextPosition, float maxDistance, Action action)
    {
        float distance = Vector2.Distance(this.npc.GetGroundPosition(), nextPosition);
        if (distance > maxDistance)
        {
            if (action != null) action.Invoke();
            return nextPosition;
        }
        else
        {
            return GlobalGameObjects.staticValues.nullVector;
        }
    }

    #endregion

    private IEnumerator delayMovementCo(float delay)
    {
        this.enableCoroutine = false;
        this.wait = true;
        yield return new WaitForSeconds(delay);
        this.wait = false;
    }

    private void MoveTroughPaths(Vector2 targetPos)
    {
        Vector2 currentPos = this.npc.GetGroundPosition();

        this.path = this.seeker.FindPath(currentPos, targetPos);
        if (this.path != null && this.index >= this.path.Count) this.path = null;

        if (path != null)
        {
            Vector2 pos = this.path[this.index];

            if (Vector2.Distance(currentPos, pos) > this.accuracy) MoveToPosition(pos);
            else this.index++;
        }
    }

    private void MoveToPosition(Vector3 position)
    {
        if (this.npc.currentState != CharacterState.knockedback
            && this.npc.currentState != CharacterState.attack
            && this.npc.currentState != CharacterState.dead
            && position != GlobalGameObjects.staticValues.nullVector)
        {
            Vector2 direction = ((Vector2)position - this.npc.GetGroundPosition()).normalized;

            Vector2 movement = new Vector2(direction.x, direction.y + (this.npc.steps * direction.x));
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

    private void SetNextWayPoint()
    {
        if (currentPoint == patrolPath.Count - 1) //letzter Knoten
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
    }

    #endregion

}
