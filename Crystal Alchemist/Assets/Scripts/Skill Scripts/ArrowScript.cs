using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : Script
{
    public override void onDestroy()
    {

    }

    public override void onUpdate()
    {
        
    }

    public override void onInitialize()
    {

    }

    public override void onExit(Collider2D hittedCharacter)
    {

    }

    public override void onStay(Collider2D hittedCharacter)
    {

    }

    public override void onEnter(Collider2D hittedCharacter)
    {       
        //Stop Arrow on Hit
        if (skill.sender != null 
            && hittedCharacter.tag != skill.sender.tag 
            && !hittedCharacter.isTrigger)
        {       
            if (!skill.playEndEffectAlready && skill.endSoundEffect != null)
            {
                Utilities.playSoundEffect(skill.audioSource, skill.endSoundEffect, skill.soundEffectVolume);
                skill.playEndEffectAlready = true;
            }

            if (!skill.rotateEndSprite) skill.transform.rotation = Quaternion.Euler(0,0,0);
            if (skill.animator != null) skill.animator.SetBool("Hit", true);
            if (skill.myRigidbody != null) skill.myRigidbody.velocity = Vector2.zero;
            skill.isActive = false;
        }
    }



}
