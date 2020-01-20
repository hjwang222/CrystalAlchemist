using UnityEngine;


public class PlayerControls : MonoBehaviour
{
    [SerializeField]
    private Player player;

    [SerializeField]
    private PlayerAttacks playerAttacks;

    [SerializeField]
    private PlayerMovement playerMovement;

    private void Update()
    {
        playerInputs();
    }

    private bool isButtonPressed(string button)
    {
        if (Input.GetButton(button)
            || Input.GetButtonUp(button)
            || Input.GetButtonDown(button)) return true;
        else return false;
    }

    private void playerInputs()
    {
        if (this.player.currentState != CharacterState.knockedback && !this.player.isOnIce)
        {
            if (this.player.myRigidbody.bodyType != RigidbodyType2D.Static) this.player.myRigidbody.velocity = Vector2.zero;
        }

        if (this.player.currentState != CharacterState.dead && this.player.currentState != CharacterState.respawning)
        {
            if (this.player.currentState == CharacterState.inDialog || this.player.currentState == CharacterState.inMenu || this.player.currentState == CharacterState.respawning)
            {
                CustomUtilities.UnityUtils.SetAnimatorParameter(this.player.animator, "isWalking", false);
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

            if (!CustomUtilities.StatusEffectUtil.isCharacterStunned(this.player))
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

            if (this.player.currentState != CharacterState.knockedback)
            {
                if (!isButtonPressed("A-Button")
                && !isButtonPressed("B-Button")
                && !isButtonPressed("X-Button")
                && !isButtonPressed("Y-Button")
                && !isButtonPressed("RB-Button")) this.playerAttacks.currentButtonPressed = "";

                this.playerAttacks.updateSkillButtons("A-Button");
                this.playerAttacks.updateSkillButtons("B-Button");
                this.playerAttacks.updateSkillButtons("X-Button");
                this.playerAttacks.updateSkillButtons("Y-Button");
                this.playerAttacks.updateSkillButtons("RB-Button");
            }
        }
    }
}
