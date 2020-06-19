using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Player))]
public class PlayerMovement : PlayerComponent
{
    private Vector2 change;
    private Vector2 position;

    #region Movement

    public void MovePlayer(InputAction.CallbackContext ctx) => this.change = ctx.ReadValue<Vector2>();

    private void FixedUpdate()
    {
        UpdateAnimationAndMove(this.change);  //check if is menu
    }

    private void UpdateAnimationAndMove(Vector2 direction)
    {
        if (this.player.values.CanMove() && direction != Vector2.zero) MoveCharacter(direction);
        else StopCharacter();        

        if (this.position != (Vector2)this.player.transform.position)
        {
            this.position = this.player.transform.position;
            AnimatorUtil.SetAnimatorParameter(this.player.animator, "isWalking", true);
            if (this.player.values.CanMove()) this.player.values.currentState = CharacterState.walk;
        }
        else
        {
            AnimatorUtil.SetAnimatorParameter(this.player.animator, "isWalking", false);
            if (this.player.values.currentState == CharacterState.walk) this.player.values.currentState = CharacterState.idle;
        }
    }

    private void StopCharacter()
    {
        if (this.player.values.currentState != CharacterState.knockedback
        && !this.player.values.isOnIce
        && this.player.myRigidbody.bodyType != RigidbodyType2D.Static) this.player.myRigidbody.velocity = Vector2.zero;
    }

    private void MoveCharacter(Vector2 direction)
    {
        if (this.player.values.currentState != CharacterState.knockedback
            && this.player.values.currentState != CharacterState.attack
            && this.player.values.currentState != CharacterState.dead
            && this.player.values.currentState != CharacterState.respawning)
        {
            Vector2 movement = new Vector2(direction.x, direction.y + (this.player.values.steps * direction.x));
            Vector2 velocity = (movement * this.player.values.speed * this.player.values.timeDistortion);
            if (!this.player.values.isOnIce) this.player.myRigidbody.velocity = velocity;
        }

        SetDirection(direction);
    }

    private void SetDirection(Vector2 direction)
    {
        if (!IsDirectionLocked()) this.player.ChangeDirection(direction);
    }

    private bool IsDirectionLocked()
    {
        foreach (Skill skill in this.player.values.activeSkills)
        {
            if (skill.isDirectionLocked()) return true;
        }
        return false;
    }

    #endregion
}
