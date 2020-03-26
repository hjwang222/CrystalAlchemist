using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    [SerializeField]
    private Player player;

    [SerializeField]
    private PlayerMovement playerMovement;

    private void Update()
    { 
        playerInputs();
    }

    private void playerInputs()
    {
        if (this.player.currentState != CharacterState.knockedback && !this.player.isOnIce)
        {
            if (this.player.myRigidbody.bodyType != RigidbodyType2D.Static) this.player.myRigidbody.velocity = Vector2.zero;
        }

        if (this.player.currentState != CharacterState.dead 
         && this.player.currentState != CharacterState.respawning)
        {
            if (this.player.currentState == CharacterState.inDialog 
             || this.player.currentState == CharacterState.inMenu 
             || this.player.currentState == CharacterState.respawning)
            {
                AnimatorUtil.SetAnimatorParameter(this.player.animator, "isWalking", false);
                return;
            }

            if (Input.GetButtonDown("Inventory"))
            {
                this.player.openInventorySignal.Raise();
            }

            if (Input.GetButtonDown("Pause"))
            {
                this.player.openPauseSignal.Raise();
            }

            if (!StatusEffectUtil.isCharacterStunned(this.player))
            {
                player.change = Vector3.zero;
                this.player.change.x = Input.GetAxisRaw("Horizontal");
                this.player.change.y = Input.GetAxisRaw("Vertical");

                if (this.player.currentState != CharacterState.dead
                    && this.player.currentState != CharacterState.inDialog
                    && this.player.currentState != CharacterState.inMenu)
                {
                    this.playerMovement.UpdateAnimationAndMove();
                }
            }            
        }
    }
}
