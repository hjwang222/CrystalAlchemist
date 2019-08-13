using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum markTargetType
{
    sender,
    target,
    skill
}

public class MarkIndicator : Indicator
{
    [SerializeField]
    private markTargetType markTarget;

    public override void Start()
    {
        base.Start();
        setPosition();
    }

    private void setPosition()
    {
        GameObject tar = getTarget();

        RectTransform rt = (RectTransform)tar.transform;
        float y = rt.rect.height;
        float scale = rt.localScale.y;

        this.transform.parent = tar.transform;
        this.transform.position = new Vector2(this.transform.position.x, this.transform.position.y + ((y * scale) / 2) + 1f);
    }

    private GameObject getTarget()
    {
        if (this.markTarget == markTargetType.target) return this.skill.target.gameObject;
        else if (this.markTarget == markTargetType.sender) return this.skill.sender.gameObject;
        else return this.skill.gameObject;
    }
}
