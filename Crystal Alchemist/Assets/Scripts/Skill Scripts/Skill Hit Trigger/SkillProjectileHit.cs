using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SkillProjectileHit : SkillHitTrigger
{
    [Space(10)]
    [InfoBox("Projektile stoppen beim Aufprall und triggern die \"Hit\"-Animation. Kein Schaden!", InfoMessageType.None)]    
    [SerializeField]
    private Skill skillOnImpact;

    private void OnTriggerEnter2D(Collider2D hittedCharacter)
    {
        stopProjectile(hittedCharacter);
    }

    private void OnTriggerStay2D(Collider2D hittedCharacter)
    {
        stopProjectile(hittedCharacter);
    }

    private void stopProjectile(Collider2D hittedCharacter)
    {
        //Stop Arrow on Hit
        if (this.skill.sender != null
            && hittedCharacter.tag != this.skill.sender.tag
            && !hittedCharacter.isTrigger
            && !hittedCharacter.CompareTag("Room"))
        {
            CustomUtilities.UnityUtils.SetAnimatorParameter(this.skill.animator, "Hit");
            // if (this.shadow != null) this.shadow.gameObject.SetActive(false);
            if (this.skill.myRigidbody != null) this.skill.myRigidbody.velocity = Vector2.zero;

            placeFire();

            this.skill.isActive = false;
        }
    }

    private void placeFire()
    {
        if (this.skillOnImpact != null)
        {
            //if (!Utilities.Collisions.checkCollision(hittedCharacter, this)) hitpoint = this.transform.position;

            GameObject fire = Instantiate(this.skillOnImpact.gameObject, this.skill.transform.position, Quaternion.identity);
            //fire.transform.position = hittedCharacter.transform.position;
            Skill fireSkill = fire.GetComponent<Skill>();

            if (fireSkill != null)
            {
                //Position nicht überschreiben
                fireSkill.overridePosition = false;
                fireSkill.sender = this.skill.sender;
            }
        }
    }
}
