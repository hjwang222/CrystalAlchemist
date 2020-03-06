using UnityEngine;
using Sirenix.OdinInspector;

public enum aoeType
{
    hide,
    range,
    look
}

public class SkillImpactHit : SkillMechanicHit
{
    [Space(10)]
    [EnumToggleButtons]
    [BoxGroup("Mechanics")]
    [SerializeField]
    private aoeType type;

    [BoxGroup("Mechanics")]
    [ShowIf("type", aoeType.look)]
    [SerializeField]
    private bool mustLookAway = false;

    [BoxGroup("Mechanics")]
    [ShowIf("type", aoeType.look)]
    [SerializeField]
    private int viewRange = 100;

    [BoxGroup("Mechanics")]
    [ShowIf("type", aoeType.range)]
    [SerializeField]
    private float deadDistance;

    [BoxGroup("Mechanics")]
    [ShowIf("type", aoeType.range)]
    [SerializeField]
    private float saveDistance;
       

    public override void hitTargetCollision(Collider2D hittedCharacter)
    {        
            if (this.type == aoeType.hide)
            {
                //hide AOE
                bool isHiding = CustomUtilities.Collisions.checkBehindObstacle(hittedCharacter.GetComponent<Character>(), this.gameObject);
                if (!isHiding) this.skill.hitIt(hittedCharacter);
            }
            else if (this.type == aoeType.range)
            {
                //range AOE
                float percentage = CustomUtilities.Collisions.checkDistanceReduce(hittedCharacter.GetComponent<Character>(),
                                                                            this.gameObject, this.deadDistance, this.saveDistance);
                this.skill.hitIt(hittedCharacter, percentage);
            }
            else if (this.type == aoeType.look)
            {
                //look /away AOE
                bool isLookingAt = CustomUtilities.Collisions.checkIfGameObjectIsViewed(hittedCharacter.GetComponent<Character>(), this.gameObject, this.viewRange);
                if ((this.mustLookAway && isLookingAt) || (!this.mustLookAway && !isLookingAt)) this.skill.hitIt(hittedCharacter);
            }
            else
            {
                //normal AOE
                this.skill.hitIt(hittedCharacter);
            }        
    }
}
