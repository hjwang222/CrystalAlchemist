using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookSkill : StandardSkill
{
    [SerializeField]
    private bool shallLookAway = true;

    public override void OnTriggerEnter2D(Collider2D hittedCharacter)
    {
        checkMechanic(hittedCharacter);
    }

    public override void OnTriggerStay2D(Collider2D hittedCharacter)
    {
        checkMechanic(hittedCharacter);
    }

    private void checkMechanic(Collider2D hittedCharacter)
    {
        //Ziel im AOE?
        if (Utilities.Collisions.checkCollision(hittedCharacter, this))
        {
            //Ziel schaut hin?
            bool isLookingAtIt = Utilities.Collisions.checkIfGameObjectIsViewed(hittedCharacter.GetComponent<Character>(), this.transform.gameObject);

            if ((shallLookAway && !isLookingAtIt) || (!shallLookAway && isLookingAtIt)) landAttack(hittedCharacter); //HIT!
        }
    }
}
