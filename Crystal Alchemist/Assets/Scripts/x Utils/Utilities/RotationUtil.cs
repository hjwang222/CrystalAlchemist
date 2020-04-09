using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationUtil : MonoBehaviour
{
    public static Quaternion getRotation(Vector2 direction)
    {
        float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
        Vector3 rotation = new Vector3(0, 0, angle);
        return Quaternion.Euler(rotation);
    }

    public static Vector2 RadianToVector2(float radian)
    {
        return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
    }

    public static Vector2 DegreeToVector2(float degree)
    {
        return RadianToVector2(degree * Mathf.Deg2Rad);
    }

    public static void rotateCollider(Character character, GameObject gameObject)
    {
        float angle = (Mathf.Atan2(character.direction.y, character.direction.x) * Mathf.Rad2Deg) + 90;

        gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    public static void setDirectionAndRotation(Skill skill, out Vector2 start, out Vector2 direction, out Vector3 rotation)
    {
        float snapRotationInDegrees = 0;
        float positionOffset = skill.GetOffset();
        float positionHeight = 0;

        SkillRotationModule rotationModule = skill.GetComponent<SkillRotationModule>();
        SkillPositionZModule positionModule = skill.GetComponent<SkillPositionZModule>();

        if (rotationModule != null)
        {
            snapRotationInDegrees = rotationModule.snapRotationInDegrees;
        }

        if (positionModule != null)
        {
            positionHeight = positionModule.positionHeight;
        }

        start = SetStartPosition(skill);
        direction = SetStartDirection(skill, start);
        start = SetOffSet(direction, start, positionOffset, positionHeight);

        float angle = SetAngle(direction, snapRotationInDegrees);
        direction = DegreeToVector2(angle);
        rotation = new Vector3(0, 0, angle);
    }

    private static Vector2 SetStartPosition(Skill skill)
    {
        if (skill.sender != null && skill.sender.skillStartPosition != null) return skill.sender.skillStartPosition.transform.position;
        else if (skill.sender != null) return skill.sender.transform.position;        
        return skill.transform.position;
    }

    private static Vector2 SetStartDirection(Skill skill, Vector2 start)
    {
        if (skill.sender == null) return skill.direction;
        else if (skill.sender.GetComponent<AI>() != null && skill.sender.GetComponent<AI>().target != null)
            return ((Vector2)skill.sender.GetComponent<AI>().target.transform.position - start).normalized;
        else if (skill.target != null)
            return ((Vector2)skill.target.transform.position - start).normalized;
        else if (skill.sender != null)
            return skill.sender.direction.normalized;

        return skill.direction;
    }

    private static Vector2 SetOffSet(Vector2 direction, Vector2 start, float offset, float positionHeight)
    {
        float positionX = start.x + (direction.x * offset);
        float positionY = start.y + (direction.y * offset) + positionHeight;

        return new Vector2(positionX, positionY);
    }

    private static float SetAngle(Vector2 direction, float snapRotationInDegrees)
    {
        float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);

        if (snapRotationInDegrees > 0)
        {
            angle = Mathf.Round(angle / snapRotationInDegrees) * snapRotationInDegrees;            
        }

        return angle;
    }
}
