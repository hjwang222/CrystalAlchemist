using UnityEngine;
using Sirenix.OdinInspector;

public class SkillRotationModule : SkillModifier
{
    public enum RotationMode
    {
        normal,
        identity,
        snap        
    }

    [BoxGroup("Rotations")]
    [Tooltip("Soll der Projektilsprite passend zur Flugbahn rotieren?")]
    [SerializeField]
    private RotationMode mode = RotationMode.normal;

    [BoxGroup("Rotations")]
    [Tooltip("Welche Winkel sollen fest gestellt werden. 0 = frei. 45 = 45° Winkel")]
    [Range(0, 90)]
    [SerializeField]
    [ShowIf("mode", RotationMode.snap)]
    private float snapRotationInDegrees = 0f;

    public void Initialize()
    {
        if (this.mode == RotationMode.identity) this.transform.rotation = Quaternion.identity;
        else if (this.mode == RotationMode.snap)
        {
            float angle = SetAngle(this.skill.direction, snapRotationInDegrees);
            this.skill.direction = RotationUtil.DegreeToVector2(angle);
            this.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }

    private float SetAngle(Vector2 direction, float snapRotationInDegrees)
    {
        float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);

        if (snapRotationInDegrees > 0)
        {
            angle = Mathf.Round(angle / snapRotationInDegrees) * snapRotationInDegrees;
        }

        return angle;
    }

    private void FixedUpdate()
    {    
        if (this.skill.spriteRenderer != null && this.skill.shadow != null) this.skill.shadow.transform.rotation = this.skill.spriteRenderer.transform.rotation;   
    }
}
