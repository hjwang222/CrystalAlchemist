using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SkillProjectileHit : SkillHitTrigger
{
    public bool canBeReflected = false;

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
            && !hittedCharacter.CompareTag("Room")
            && !isReflected(hittedCharacter))
        {
            AnimatorUtil.SetAnimatorParameter(this.skill.animator, "Hit");
            this.skill.GetComponent<SkillProjectile>().stopVelocity();

            placeFire();
        }
    }

    private bool isReflected(Collider2D hittedCharacter)
    {
        if (hittedCharacter.GetComponent<SkillCollider2DHelper>() != null
            && hittedCharacter.GetComponent<SkillCollider2DHelper>().skill.GetComponent<SkillReflector>() != null
            && this.canBeReflected) return true;

        return false;
    }

    private void placeFire()
    {
        if (this.skillOnImpact != null)
        {
            GameObject fire = Instantiate(this.skillOnImpact.gameObject, this.skill.transform.position, Quaternion.identity);
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
