using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public enum aoeType
{
    hide,
    range,
    look
}

public class SkillImpactHit : SkillHitTrigger
{ 
        [SerializeField]
        [Required]
        private StandardSkill skill;

        [Space(10)]
        [EnumToggleButtons]
        [FoldoutGroup("Special Behaviors", expanded: false)]
        [SerializeField]
        private aoeType type;

        [FoldoutGroup("Special Behaviors", expanded: false)]
        [ShowIf("type", aoeType.look)]
        [SerializeField]
        private bool mustLookAway = false;

        [FoldoutGroup("Special Behaviors", expanded: false)]
        [ShowIf("type", aoeType.look)]
        [SerializeField]
        int viewRange = 45;

        [FoldoutGroup("Special Behaviors", expanded: false)]
        [ShowIf("type", aoeType.range)]
        [SerializeField]
        private Vector2 rangePercentage;
    
        private void OnTriggerEnter2D(Collider2D hittedCharacter)
        {
            checkMechanics(hittedCharacter);
        }

        private void checkMechanics(Collider2D hittedCharacter)
        {
            if (Utilities.Collisions.checkCollision(hittedCharacter, this.skill))
            {
                if (this.type == aoeType.hide)
                {
                    //hide AOE
                    bool isHiding = Utilities.Collisions.checkBehindObstacle(hittedCharacter.GetComponent<Character>(), this.gameObject);
                    if (!isHiding) this.skill.hitIt(hittedCharacter);
                }
                else if (this.type == aoeType.range)
                {
                    //range AOE
                    float percentage = Utilities.Collisions.checkDistanceReduce(hittedCharacter.GetComponent<Character>(),
                                                                                this.gameObject, rangePercentage.x, rangePercentage.y);
                    this.skill.hitIt(hittedCharacter, percentage);
                }
                else if (this.type == aoeType.look)
                {
                    //normal AOE
                    bool isLookingAt = Utilities.Collisions.checkIfGameObjectIsViewed(hittedCharacter.GetComponent<Character>(), this.gameObject, this.viewRange);
                    if ((this.mustLookAway && isLookingAt) || (!this.mustLookAway && !isLookingAt)) this.skill.hitIt(hittedCharacter);
                }
                else
                {
                    //normal AOE
                    this.skill.hitIt(hittedCharacter);
                }
            }
        }
    }
