using System.Collections.Generic;
using UnityEngine;


public class CollisionUtil : MonoBehaviour
{
    public static bool checkDistance(Character character, GameObject gameObject, float min, float max, float startDistance, float distanceNeeded, bool useStartDistance, bool useRange)
    {
        float distance = Vector3.Distance(character.transform.position, gameObject.transform.position);

        //Debug.Log(startDistance + " : " + distanceNeeded + " : "+distance);

        if (useStartDistance && distanceNeeded > 0)
        {
            if (distance > (startDistance + distanceNeeded)) return true;
        }
        else if (!useStartDistance && distanceNeeded > 0)
        {
            if (distance > distanceNeeded) return true;
        }
        else if (useRange)
        {
            if (distance >= min && distance <= max) return true;
        }

        return false;
    }

    public static float checkDistanceReduce(Character character, GameObject gameObject, float deadDistance, float saveDistance)
    {
        float distance = Vector3.Distance(character.transform.position, gameObject.transform.position);
        float percentage = 100 - (100 / (saveDistance - deadDistance) * (distance - deadDistance));

        percentage = Mathf.Round(percentage / 25) * 25;

        if (percentage > 100) percentage = 100;
        else if (percentage < 0) percentage = 0;

        return percentage;
    }

    public static bool checkBehindObstacle(Character character, GameObject gameObject)
    {
        float offset = 0.1f;
        Vector2 targetPosition = new Vector2(character.transform.position.x - (character.direction.x * offset),
                                                 character.transform.position.y - (character.direction.y * offset));


        if (character.shadowRenderer != null)
        {
            targetPosition = new Vector2(character.shadowRenderer.transform.position.x - (character.direction.x * offset),
                                         character.shadowRenderer.transform.position.y - (character.direction.y * offset));
        }

        Vector2 start = gameObject.transform.position;
        Vector2 direction = (targetPosition - start).normalized;

        RaycastHit2D hit = Physics2D.Raycast(start, direction, 100f);

        if (hit && !hit.collider.isTrigger)
        {
            if (hit.collider.gameObject != character.gameObject)
            {
                //Debug.DrawLine(start, hit.transform.position, Color.green);
                return true;
            }
            else
            {
                //Debug.DrawLine(start, hit.transform.position, Color.red);
                return false;
            }
        }


        return true;
    }

    public static bool checkAffections(Character sender, bool affectOther, bool affectSame, bool affectNeutral, Collider2D hittedCharacter)
    {
        Character target = hittedCharacter.GetComponent<Character>();

        if (!hittedCharacter.isTrigger
            && target != null
            && target.currentState != CharacterState.dead
            && target.currentState != CharacterState.respawning)
        {
            return checkMatrix(sender, target, affectOther, affectSame, affectNeutral);
        }

        return false;
    }

    private static bool checkMatrix(Character sender, Character target, bool other, bool same, bool neutral)
    {
        if (other)
        {
            if (sender == null) return true;
            if (sender.stats.characterType == CharacterType.Friend && target.stats.characterType == CharacterType.Enemy) return true;
            if (sender.stats.characterType == CharacterType.Enemy && target.stats.characterType == CharacterType.Friend) return true;
        }

        if (same)
        {
            if (sender == null) return true;
            if (sender.stats.characterType == target.stats.characterType) return true;
        }

        if (neutral)
        {
            if (target.stats.characterType == CharacterType.Object) return true;
        }

        return false;
    }

    public static List<Character> getAffectedCharacters(Skill skill)
    {
        List<GameObject> result = new List<GameObject>();
        List<Character> targets = new List<Character>();
        SkillTargetModule targetModule = skill.GetComponent<SkillTargetModule>();

        if (targetModule != null)
        {
            if ((skill.sender.CompareTag("Player") || skill.sender.CompareTag("NPC")) && targetModule.affectOther)
            {
                result.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
            }
            if (skill.sender.CompareTag("Enemy") && targetModule.affectOther)
            {
                result.AddRange(GameObject.FindGameObjectsWithTag("Player"));
                result.AddRange(GameObject.FindGameObjectsWithTag("NPC"));
            }
            if (targetModule.affectSame)
            {
                result.AddRange(GameObject.FindGameObjectsWithTag(skill.sender.tag));
            }
            if (targetModule.affectNeutral)
            {
                result.AddRange(GameObject.FindGameObjectsWithTag("Other"));
            }
        }

        foreach (GameObject res in result)
        {
            if (res.GetComponent<Character>() != null) targets.Add(res.GetComponent<Character>());
        }

        return targets;
    }

    private static bool skillAffected(Collider2D hittedCharacter, Skill skill)
    {
        Skill tempSkill = AbilityUtil.getSkillByCollision(hittedCharacter.gameObject);

        if (tempSkill != null)
        {
            if (skill.GetComponent<SkillTargetModule>() != null
            && skill.GetComponent<SkillTargetModule>().affectSkills
            && tempSkill.CompareTag("Skill")
            && tempSkill.name != skill.name)
            {
                return true;
            }
        }
        return false;
    }

    public static bool checkCollision(Collider2D hittedCharacter, Skill skill)
    {
        return checkCollision(hittedCharacter, skill, skill.sender);
    }

    public static bool checkCollision(Collider2D hittedCharacter, Skill skill, Character sender)
    {
        if (skill != null && skill.GetTriggerActive())
        {
            if (sender != null && hittedCharacter.gameObject == sender.gameObject)
            {
                if (skill.GetComponent<SkillTargetModule>() != null
                    && skill.GetComponent<SkillTargetModule>().affectSelf) return true;
                else return false;
            }
            else
            {
                if (skillAffected(hittedCharacter, skill))
                {
                    return true;
                }
                else if (!hittedCharacter.isTrigger)
                {
                    SkillTargetModule targetModule = skill.GetComponent<SkillTargetModule>();
                    if (targetModule != null
                        && checkAffections(sender, targetModule.affectOther, targetModule.affectSame, targetModule.affectNeutral, hittedCharacter))
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    public static bool checkIfGameObjectIsViewed(Character character, GameObject target, int range)
    {
        Vector2 direction = character.direction;
        Vector2 temp = (character.transform.position - target.transform.position).normalized;

        float direction_angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        float temp_angle = Mathf.Atan2(temp.x, temp.y) * Mathf.Rad2Deg;

        float angle = Mathf.Abs(direction_angle - temp_angle);

        float min = 180 - range;
        float max = 180 + range;

        //Debug.Log(angle + ">>" + min + ":" + max);

        if (angle >= min && angle <= max) return true;
        return false;
    }

    public static bool checkIfGameObjectIsViewed(Character character, GameObject gameObject)
    {
        return checkIfGameObjectIsViewed(character, gameObject, 1f);
    }

    public static bool checkIfGameObjectIsViewed(Character character, GameObject gameObject, float distance)
    {
        if (character != null && character.shadowRenderer == null)
        {
            Debug.Log("Schatten-Objekt ist leer!");
            return false;
        }

        float width = 0.2f;
        float offset = 0.1f;

        Vector2 position = new Vector2(character.shadowRenderer.transform.position.x - (character.direction.x * offset),
                                       character.shadowRenderer.transform.position.y - (character.direction.y * offset));

        RaycastHit2D[] hit = Physics2D.CircleCastAll(position, width, character.direction, distance);

        foreach (RaycastHit2D hitted in hit)
        {
            if (hitted != false
                && !hitted.collider.isTrigger
                && hitted.collider.transform.parent != null
                && hitted.collider.transform.parent.gameObject == gameObject) return true;
        }

        return false;
    }
}
