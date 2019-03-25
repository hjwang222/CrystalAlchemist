using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSkill : StandardSkill
{
    public override void OnTriggerExit2D(Collider2D hittedCharacter)
    {
        base.OnTriggerExit2D(hittedCharacter);
        //Stop Arrow on Hit
        if (this.sender != null 
            && hittedCharacter.tag != this.sender.tag 
            && !hittedCharacter.isTrigger)
        {       
            if (!this.playEndEffectAlready && this.endSoundEffect != null)
            {
                Utilities.playSoundEffect(this.audioSource, this.endSoundEffect, this.soundEffectVolume);
                this.playEndEffectAlready = true;
            }

            if (!this.rotateEndSprite) this.transform.rotation = Quaternion.Euler(0,0,0);
            if (this.animator != null) this.animator.SetBool("Hit", true);
            if (this.myRigidbody != null) this.myRigidbody.velocity = Vector2.zero;
            this.isActive = false;
        }
    }



}
