using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : Character
{
    private void setAnimFloat(Vector2 setVector)
    {
        Utilities.UnityUtils.SetAnimatorParameter(this.animator, "moveX", setVector.x);
        Utilities.UnityUtils.SetAnimatorParameter(this.animator, "moveY", setVector.y);
    }

    public void changeAnim(Vector2 direction)
    {
        //TODO: To be tested
        this.direction = direction;

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            if (direction.x > 0) setAnimFloat(Vector2.right);
            else if (direction.x < 0) setAnimFloat(Vector2.left);
        }
        else if (Mathf.Abs(direction.x) < Mathf.Abs(direction.y))
        {
            if (direction.y > 0) setAnimFloat(Vector2.up);
            else if (direction.y < 0) setAnimFloat(Vector2.down);
        }
    }
}
