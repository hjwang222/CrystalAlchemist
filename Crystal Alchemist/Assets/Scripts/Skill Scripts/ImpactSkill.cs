using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactSkill : StandardSkill
{
    [SerializeField]
    private bool mustHide = false;

    [SerializeField]
    private bool mustBreakLine = false;

    [SerializeField]
    private Vector2 rangePercentage;


    public override void OnTriggerEnter2D(Collider2D hittedCharacter)
    {
        
    }

    public override void OnTriggerStay2D(Collider2D hittedCharacter)
    {
        
    }

    private void checkMechanics(Collider2D hittedCharacter)
    {
        if (Utilities.Collisions.checkCollision(hittedCharacter, this))
        {
            //check
            bool isHiding = Utilities.Collisions.checkBehindObstacle(hittedCharacter.GetComponent<Character>(), this.transform.gameObject);

            //update Target Resource Affections
            float percentage = Utilities.Collisions.checkDistanceReduce(hittedCharacter.GetComponent<Character>(),
                                                                        this.transform.gameObject, rangePercentage.x, rangePercentage.y);
        }
    }


}
