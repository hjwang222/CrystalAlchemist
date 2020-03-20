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

    public static void setDirectionAndRotation(Skill skill, out float angle, out Vector2 start, out Vector2 direction, out Vector3 rotation)
    {
        float snapRotationInDegrees = 0;
        float rotationModifier = 0;
        float positionOffset = skill.positionOffset;
        float positionHeight = 0;

        SkillRotationModule rotationModule = skill.GetComponent<SkillRotationModule>();
        SkillPositionZModule positionModule = skill.GetComponent<SkillPositionZModule>();

        if (rotationModule != null)
        {
            snapRotationInDegrees = rotationModule.snapRotationInDegrees;
            rotationModifier = rotationModule.rotationModifier;
        }

        if (positionModule != null)
        {
            positionHeight = positionModule.positionHeight;
        }

        start = (Vector2)skill.sender.transform.position;

        if (skill.sender.GetComponent<AI>() != null && skill.sender.GetComponent<AI>().target != null)
            direction = ((Vector2)skill.sender.GetComponent<AI>().target.transform.position - start).normalized;
        else if (skill.target != null)
            direction = ((Vector2)skill.target.transform.position - start).normalized;
        else
            direction = skill.sender.direction.normalized;

        float positionX = skill.sender.skillStartPosition.transform.position.x + (direction.x * positionOffset);
        float positionY = skill.sender.skillStartPosition.transform.position.y + (direction.y * positionOffset) + positionHeight;

        //if (useCustomPosition) positionY = skill.sender.shootingPosition.transform.position.y + (direction.y * positionOffset);

        start = new Vector2(positionX, positionY);
        if (skill.sender.GetComponent<AI>() != null && skill.sender.GetComponent<AI>().target != null) direction = (Vector2)skill.sender.GetComponent<AI>().target.transform.position - start;
        else if (skill.target != null) direction = (Vector2)skill.target.transform.position - start;


        float temp_angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        direction = RotationUtil.DegreeToVector2(temp_angle);

        angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) + rotationModifier;

        if (snapRotationInDegrees > 0)
        {
            angle = Mathf.Round(angle / snapRotationInDegrees) * snapRotationInDegrees;
            direction = RotationUtil.DegreeToVector2(angle);
        }

        rotation = new Vector3(0, 0, angle);
    }
}
