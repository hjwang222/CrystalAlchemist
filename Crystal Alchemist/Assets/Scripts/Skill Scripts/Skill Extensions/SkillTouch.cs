using UnityEngine;

public class SkillTouch : SkillExtension
{
    private void Start()
    {
        Collider2D temp = this.skill.sender.boxCollider;

        Collider2D te = CustomUtilities.UnityUtils.CopyComponent(temp, this.gameObject);
        te.isTrigger = true;
    }
}
