using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RotationUtil
{
    public static Quaternion getRotation(Vector2 direction)
    {
        float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
        Vector3 rotation = new Vector3(0, 0, angle);
        return Quaternion.Euler(rotation);
    }

    public static Vector2 RadianToVector2(float radian)
    {
        float x = Mathf.Round((Mathf.Cos(radian) * 100f)) / 100f;
        float y = Mathf.Round((Mathf.Sin(radian) * 100f)) / 100f;

        return new Vector2(x, y).normalized;
    }

    public static Vector2 DegreeToVector2(float degree)
    {
        return RadianToVector2(degree * Mathf.Deg2Rad);
    }

    public static void rotateCollider(Character character, GameObject gameObject)
    {
        float angle = (Mathf.Atan2(character.values.direction.y, character.values.direction.x) * Mathf.Rad2Deg) + 90;

        gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    public static Vector2 SetStartDirection(Skill skill)
    {
        if (skill.sender == null) return skill.GetDirection();
        else if (skill.sender.GetComponent<AI>() != null && skill.sender.GetComponent<AI>().target != null)
            return (skill.sender.GetComponent<AI>().target.GetGroundPosition() - (Vector2)skill.transform.position).normalized;
        else if (skill.target != null)
            return (skill.target.GetGroundPosition() - (Vector2)skill.transform.position).normalized;
        else if (skill.sender != null)
            return skill.sender.values.direction.normalized;

        return skill.GetDirection();
    }
}
