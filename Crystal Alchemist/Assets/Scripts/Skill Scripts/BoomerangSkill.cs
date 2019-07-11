using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangSkill : StandardSkill
{
    #region Attributes
    [Tooltip("Zeitpunkt der Scriptaktivierung")]
    [Range(0, Utilities.maxFloatSmall)]
    public float timeToMoveBack = 0;
    private float durationThenBackToSender = 0;

    [SerializeField]
    private float minDistance = 0.1f;
    private Item moveItem;
    #endregion


    #region Overrides
    public override void init()
    {
        base.init();
        this.durationThenBackToSender = timeToMoveBack;
    }

    public override void doOnUpdate()
    {
        base.doOnUpdate();

        if (this.durationThenBackToSender > 0)
        {
            this.durationThenBackToSender -= (Time.deltaTime * this.timeDistortion);
        }
        else
        {
            moveBackToSender();
        }        
    }

    public override void OnTriggerEnter2D(Collider2D hittedCharacter)
    {
        base.OnTriggerEnter2D(hittedCharacter);

        moveThings(hittedCharacter);
    }

    public override void OnTriggerStay2D (Collider2D hittedCharacter)
    {
        base.OnTriggerStay2D(hittedCharacter);
        //got Hit -> Back to Target
        moveThings(hittedCharacter);
    }

    private void moveThings(Collider2D hittedCharacter)
    {
        if (this.sender != null
            && hittedCharacter.tag != this.sender.tag
            && !hittedCharacter.isTrigger
            && !hittedCharacter.CompareTag("Object"))
        {
            this.durationThenBackToSender = 0;
        }

        if (this.sender != null
            && hittedCharacter.tag != this.sender.tag
            && hittedCharacter.CompareTag("Item"))
        {
            this.durationThenBackToSender = 0;

            Item hittedItem = hittedCharacter.GetComponent<Item>();
            if (hittedItem != null && this.moveItem == null) this.moveItem = hittedItem;        
        }
    }
    #endregion


    #region Functions (private)
    private void moveBackToSender()
    {
        if (this.sender != null)
        {
            //Bewege den Skill zurück zum Sender

            this.myRigidbody.velocity = Vector2.zero;
            if (Vector3.Distance(this.sender.transform.position, this.transform.position) > this.minDistance)
            {
                Vector3 newPosition = Vector3.MoveTowards(this.transform.position, this.sender.transform.position, this.speed * (Time.deltaTime * this.timeDistortion));

                this.myRigidbody.MovePosition(newPosition);
                this.myRigidbody.velocity = Vector2.zero;

                if(this.moveItem != null && this.moveItem.GetComponent<Rigidbody2D>() != null)
                {
                    this.moveItem.GetComponent<Rigidbody2D>().MovePosition(newPosition);
                }
            }
            else
            {
                this.DestroyIt();
            }
        }
    }
    #endregion
}
