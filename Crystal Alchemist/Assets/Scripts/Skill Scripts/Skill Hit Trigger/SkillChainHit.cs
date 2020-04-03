using UnityEngine;
using Sirenix.OdinInspector;

public class SkillChainHit : SkillMechanicHit
{
    [BoxGroup("Mechanics")]
    public bool useRange = false;

    [SerializeField]
    [ShowIf("useRange", true)]
    [BoxGroup("Mechanics")]
    private Vector2 rangeNeeded;

    [SerializeField]
    [HideIf("useRange", true)]
    [BoxGroup("Mechanics")]
    private float distanceNeeded = 0f;

    [HideIf("useRange", true)]
    [BoxGroup("Mechanics")]
    public bool canBreak = false;

    [HideIf("useRange", true)]
    [BoxGroup("Mechanics")]
    [SerializeField]
    private bool useStartDistance = false;

    [BoxGroup("Mechanics")]
    public bool changeColor = false;

    [BoxGroup("Mechanics")]
    [ShowIf("changeColor", true)]
    public Color rightColor;

    [BoxGroup("Mechanics")]
    [ShowIf("changeColor", true)]
    public Color wrongColor;

    private float startDistance = 0;

    private void Update()
    {
        if (this.hasRightDistance())
        {
            this.percentage = 0;

            if (this.canBreak && !this.useRange) this.skill.DeactivateIt();            
        }
        else
        {
            this.percentage = 100;          
        }
    }

    public bool hasRightDistance()
    {
        if (this.skill.target != null)
        {
            if (this.useStartDistance && this.startDistance <= 0)
                this.startDistance = Vector3.Distance(this.skill.target.transform.position, this.skill.sender.transform.position);

            return CollisionUtil.checkDistance(this.skill.target,
                                                      this.skill.sender.gameObject, rangeNeeded.x, rangeNeeded.y,
                                                      this.startDistance, this.distanceNeeded,
                                                      this.useStartDistance, this.useRange);
        }

        return false;
    }
}
