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
        this.transform.position = SetOffSet(this.skill.direction, this.transform.position, this.skill.positionOffset, positionHeight);

        if (this.skill.shadow != null)
        {
            float changeX = 0;
            if (this.skill.direction.y < 0) changeX = this.skill.direction.y;
        
            this.skill.shadow.transform.position = new Vector2(this.transform.position.x, this.transform.position.y + colliderHeightOffset + (colliderHeightOffset * changeX));
        }
    }

    private Vector2 SetOffSet(Vector2 direction, Vector2 start, float offset, float positionHeight)
    {
        float positionX = start.x + (direction.x * offset);
        float positionY = start.y + (direction.y * offset) + positionHeight;

        return new Vector2(positionX, positionY);
    }
}
