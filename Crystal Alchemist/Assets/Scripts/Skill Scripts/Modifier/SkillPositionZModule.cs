using UnityEngine;
using Sirenix.OdinInspector;

public class SkillPositionZModule : SkillModifier
{
    [BoxGroup("Position Z")]
    [Range(-1, 2)]
    [Tooltip("Positions-Höhe")]
    public float positionHeight = 0f;

    [BoxGroup("Position Z")]
    [Tooltip("Schattencollider Höhe")]
    [Range(-1, 0)]
    public float colliderHeightOffset = -0.5f;

    public void Initialize()
    {
        this.transform.position = new Vector2(this.transform.position.x, this.transform.position.y + positionHeight);

        if (this.skill.shadow != null)
        {
            float changeX = 0;
            if (this.skill.GetDirection().y < 0) changeX = this.skill.GetDirection().y;
        
            this.skill.shadow.transform.position = new Vector2(this.transform.position.x, this.transform.position.y + colliderHeightOffset + (colliderHeightOffset * changeX));
        }
    }
}
