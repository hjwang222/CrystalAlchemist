using System.Collections.Generic;
using UnityEngine;

public class TestMovement : MonoBehaviour
{
    [SerializeField]
    private GameObject follow;

    [SerializeField]
    private float speed = 2f;

    [SerializeField]
    private float maxDistance = 0.1f;

    [SerializeField]
    private PathSeeker seeker;

    [SerializeField]
    private float seekerAccuracy = 0.25f;

    private List<Vector2> path; //Required
    private int index; //Required
    private Rigidbody2D myRigidbody2D; //Required

    private void Start()
    {
        this.myRigidbody2D = this.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        this.myRigidbody2D.velocity = Vector2.zero; 
        if (this.follow != null && this.seeker != null)
            MoveTroughPaths(this.transform.position, follow.transform.position);
    }

    private void MoveTroughPaths(Vector2 currentPos, Vector2 targetPos)
    {
        this.path = this.seeker.FindPath(currentPos, targetPos); //get path from pathfinder
        if (this.path != null && this.index >= this.path.Count) this.path = null; //reached last pathnode

        if (path != null)
        {
            Vector2 pos = this.path[this.index];
            GizmoUtil.ShowWalkingLines(this.path); //for Debugging purpose

            if (Vector2.Distance(currentPos, pos) > this.seekerAccuracy) MoveToPosition(pos); //move to pathnode
            else this.index++; //set next pathnode
        }
    }

    private void MoveToPosition(Vector2 position)
    {
        Vector2 direction = (position - (Vector2)this.transform.position).normalized;
        this.myRigidbody2D.velocity = (direction * this.speed);
    }
}
