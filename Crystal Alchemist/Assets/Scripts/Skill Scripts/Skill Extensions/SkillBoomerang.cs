using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SkillBoomerang : SkillProjectile
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
            this.durationThenBackToSender -= (Time.deltaTime * this.skill.getTimeDistortion());
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
            if (Vector3.Distance(this.skill.sender.transform.position, this.transform.position) > this.minDistance)
            { 
                this.skill.direction = this.skill.sender.transform.position - this.transform.position;
                this.setVelocity();
            }
            else
            {
                this.skill.DestroyIt();
            }
        }
    }
    #endregion
}
