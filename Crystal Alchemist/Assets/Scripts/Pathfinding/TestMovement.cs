using System.Collections.Generic;
using UnityEngine;

public class TestMovement : MonoBehaviour
{
    [SerializeField]
    private AI npc;

    [SerializeField]
    private float maxDistance;

    [SerializeField]
    private PathSeeker seeker;

    private List<Vector2> path;
    private int index;

    private void Update()
    {
        moveCharacter();  
    }

    private void moveCharacter()
    {
        if (this.npc.currentState != CharacterState.knockedback && !this.npc.isOnIce)
        {
            if (this.npc.myRigidbody.bodyType != RigidbodyType2D.Static) this.npc.myRigidbody.velocity = Vector2.zero;
        }

        Vector3 targetPosition = CheckDistance();

        if (targetPosition != GlobalGameObjects.staticValues.nullVector)
        {
            movePath(this.npc.GetGroundPosition(), targetPosition);
        }        
    }

    public void ShowLines(List<Vector2> path)
    {
        if (path != null)
        {
            for (int i = 0; i < path.Count - 1; i++)
            {
                Debug.DrawLine(path[i], path[i+1], Color.green);
            }
        }
    }


    private void movePath(Vector2 currentPos, Vector2 targetpos)
    {
        this.path = this.seeker.FindPath(currentPos, targetpos);
        if (this.path != null && this.index >= this.path.Count) this.path = null;        

        if (path != null)
        {
            if (Pathfinding.Instance.showDebug) ShowLines(path);
            Vector2 pos = this.path[this.index];

            if (Vector2.Distance(currentPos, pos) > 0.25f) MoveToPosition(pos);            
            else this.index++;             
        }
    }

    private Vector3 CheckDistance()
    {
        if (this.npc.target != null)
        {
            float distance = Vector2.Distance(this.npc.GetGroundPosition(), this.npc.target.GetGroundPosition());
            if (distance > maxDistance)
            {
                return this.npc.target.GetGroundPosition();
            }
        }

        return GlobalGameObjects.staticValues.nullVector;
    }

    private void MoveToPosition(Vector3 position)
    {
        if (this.npc.currentState != CharacterState.knockedback
            && this.npc.currentState != CharacterState.attack
            && this.npc.currentState != CharacterState.dead
            && position != GlobalGameObjects.staticValues.nullVector)
        {
            this.npc.direction = ((Vector2)position - this.npc.GetGroundPosition()).normalized;

            Vector2 movement = new Vector2(this.npc.direction.x, this.npc.direction.y + (this.npc.steps * this.npc.direction.x));
            if (!this.npc.isOnIce) this.npc.myRigidbody.velocity = (movement * this.npc.speed * this.npc.timeDistortion);
        }
    }
}
