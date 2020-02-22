using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private Player player;

    #region Movement

    public void UpdateAnimationAndMove()
    {
        if (this.player.change != Vector3.zero)
        {
            MoveCharacter();

            bool lockAnimation = false;

            foreach (Skill skill in this.player.activeSkills)
            {
                if (skill.movementLocked)
                {
                    lockAnimation = true;
                    break;
                }
            }

            if (!lockAnimation)
            {
                this.player.direction = this.player.change;
                this.player.updateAnimDirection(this.player.change);
            }

            CustomUtilities.UnityUtils.SetAnimatorParameter(this.player.animator, "isWalking", true);
        }
        else
        {
            CustomUtilities.UnityUtils.SetAnimatorParameter(this.player.animator, "isWalking", false);
            if (this.player.currentState == CharacterState.walk) this.player.currentState = CharacterState.idle;
        }
    }

    private void MoveCharacter()
    {
        if (this.player.currentState != CharacterState.knockedback
            && this.player.currentState != CharacterState.attack
            && this.player.currentState != CharacterState.dead)
        {
            if (this.player.currentState != CharacterState.interact) this.player.currentState = CharacterState.walk;
            this.player.change.Normalize(); //Diagonal-Laufen fixen

            //this.myRigidbody.MovePosition(transform.position + change * this.speed * (Time.deltaTime * this.timeDistortion));
            //this.myRigidbody.velocity = Vector2.zero;

            Vector3 movement = new Vector3(this.player.change.x, this.player.change.y + (this.player.steps * this.player.change.x), 0.0f);
            if (!this.player.isOnIce) this.player.myRigidbody.velocity = (movement * this.player.speed * this.player.timeDistortion);
        }
    }

    #endregion
}
