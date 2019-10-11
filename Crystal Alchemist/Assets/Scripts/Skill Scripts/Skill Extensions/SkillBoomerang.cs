using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SkillBoomerang : MonoBehaviour
{
    [SerializeField]
    [Required]
    private StandardSkill skill;

    #region Attributes
    [Tooltip("Zeitpunkt der Scriptaktivierung")]
    [Range(0, Utilities.maxFloatSmall)]
    public float timeToMoveBack = 0;
    private float durationThenBackToSender = 0;

    [SerializeField]
    private float minDistance = 0.1f;
    private Item moveItem;
    private bool speedup = true;
    private Vector2 tempVelocity;
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

    public void moveThings(Collider2D hittedCharacter)
    {
        if (this.skill.sender != null
            && hittedCharacter.tag != this.skill.sender.tag
            && !hittedCharacter.isTrigger
            && !hittedCharacter.CompareTag("Object"))
        {
            this.durationThenBackToSender = 0;
        }

        if (this.skill.sender != null
            && hittedCharacter.tag != this.skill.sender.tag
            && hittedCharacter.CompareTag("Item"))
        {
            this.durationThenBackToSender = 0;

            Item hittedItem = hittedCharacter.GetComponent<Item>();
            if (hittedItem != null && this.moveItem == null) this.moveItem = hittedItem;
        }
    }

    private void OnTriggerEnter2D(Collider2D hittedCharacter)
    {
        moveThings(hittedCharacter);
    }

    private void OnTriggerStay2D(Collider2D hittedCharacter)
    {
        //got Hit -> Back to Target
        moveThings(hittedCharacter);
    }


    #region Functions (private)
    private void moveBackToSender()
    {
        if (this.skill.sender != null)
        {
            //Bewege den Skill zurück zum Sender

            this.skill.myRigidbody.velocity = Vector2.zero;
            if (Vector3.Distance(this.skill.sender.transform.position, this.transform.position) > this.minDistance)
            {
                Vector3 newPosition = Vector3.MoveTowards(this.transform.position, this.skill.sender.transform.position, this.skill.speed * (Time.deltaTime * this.skill.timeDistortion));

                this.skill.myRigidbody.MovePosition(newPosition);
                this.skill.myRigidbody.velocity = Vector2.zero;

                if (this.moveItem != null && this.moveItem.GetComponent<Rigidbody2D>() != null)
                {
                    this.moveItem.GetComponent<Rigidbody2D>().MovePosition(newPosition);
                }
            }
            else
            {
                this.skill.DestroyIt();
            }
        }
    }
    #endregion
}
