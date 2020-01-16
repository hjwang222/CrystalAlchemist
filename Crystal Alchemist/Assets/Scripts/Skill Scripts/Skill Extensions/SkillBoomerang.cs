using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SkillBoomerang : SkillExtension
{
    #region Attributes
    [Tooltip("Zeitpunkt der Scriptaktivierung")]
    [Range(0, CustomUtilities.maxFloatSmall)]
    public float timeToMoveBack = 0;

    [HideInInspector]
    public float durationThenBackToSender = 0;

    [SerializeField]
    private float minDistance = 0.1f;

    private bool speedup = true;
    //private Vector2 tempVelocity;
    #endregion
    

    private void Start()
    {
        this.durationThenBackToSender = timeToMoveBack;
    }

    private void Update()
    {
        if (this.durationThenBackToSender > 0)
        {
            this.durationThenBackToSender -= (Time.deltaTime * this.skill.timeDistortion);
        }
        else
        {
            moveBackToSender();
        }
    }
    private void OnTriggerEnter2D(Collider2D hittedCharacter)
    {
        checkHit(hittedCharacter);
    }

    private void OnTriggerStay2D(Collider2D hittedCharacter)
    {
        //got Hit -> Back to Target
        checkHit(hittedCharacter);
    }


    public void checkHit(Collider2D hittedCharacter)
    {
        if (this.skill.sender != null
            && hittedCharacter.tag != this.skill.sender.tag
            && ((!hittedCharacter.isTrigger
                 && !hittedCharacter.CompareTag("Object"))
               || hittedCharacter.CompareTag("Item")))
        {
            if (this.GetComponent<SkillBoomerang>() != null) this.GetComponent<SkillBoomerang>().durationThenBackToSender = 0;
        }
    }

    #region Functions (private)
    private void moveBackToSender()
    {
        if (this.skill.sender != null)
        {
            //Bewege den Skill zurück zum Sender

            //this.skill.myRigidbody.velocity = Vector2.zero;
            if (Vector3.Distance(this.skill.sender.transform.position, this.transform.position) > this.minDistance)
            {
                //Vector3 newPosition = Vector3.MoveTowards(this.transform.position, this.skill.sender.transform.position, this.skill.speed * (Time.deltaTime * this.skill.timeDistortion));

                //this.skill.myRigidbody.MovePosition(newPosition);
                //this.skill.myRigidbody.velocity = Vector2.zero;     

                this.skill.direction = this.skill.sender.transform.position - this.transform.position;
                this.skill.myRigidbody.velocity = this.skill.direction.normalized * this.skill.speed;
                //this.tempVelocity = this.skill.myRigidbody.velocity;

            }
            else
            {
                this.skill.DestroyIt();
            }
        }
    }
    #endregion
}
