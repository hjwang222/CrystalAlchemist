using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CollisionUtil : MonoBehaviour
{
    public static bool checkDistance(Character character, GameObject gameObject, float min, float max, float startDistance, float distanceNeeded, bool useStartDistance, bool useRange)
    {
        float distance = Vector3.Distance(character.transform.position, gameObject.transform.position);

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
        Vector2 targetPosition = new Vector2(character.GetGroundPosition().x - (character.direction.x * offset),
                                             character.GetGroundPosition().y - (character.direction.y * offset));

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

    public static List<Character> getAllAffectedCharacters(Skill skill)
    {
        List<Character> targets = new List<Character>();
        SkillTargetModule targetModule = skill.GetComponent<SkillTargetModule>();

        if (targetModule != null)
        {
            List<Character> found = FindObjectsOfType<Character>().ToList();

            foreach(Character character in found)
            {
                if (checkMatrix(skill.sender, character, targetModule.affectOther, targetModule.affectSame, targetModule.affectNeutral))
                    targets.Add(character);
            }
            
        }

        return targets;
    }

    public static bool checkCollision(Collider2D hittedCharacter, Skill skill)
    {
        return checkCollision(hittedCharacter, skill, skill.sender);
    }

    public static bool checkCollision(Collider2D other, Skill skill, Character sender)
    {
        if (skill != null && skill.GetTriggerActive())
        {
            SkillTargetModule targetModule = skill.GetComponent<SkillTargetModule>();

            if (targetModule != null)
            {
                Character hittedCharacter = null;
                if (!other.isTrigger) hittedCharacter = other.GetComponent<Character>();

                if (hittedCharacter != null)
                {
                    if (targetModule.affectSelf && hittedCharacter == sender) return true;
                    if (checkAffections(sender, targetModule.affectOther, targetModule.affectSame, targetModule.affectNeutral, other)) return true;
                }

                Skill hittedSkill = AbilityUtil.getSkillByCollision(other.gameObject);

                if (hittedSkill != null)
                {
                    if (targetModule.affectSkills && hittedSkill != skill) return true;
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
        float width = 0.2f;
        float offset = 0.1f;

        Vector2 position = new Vector2(character.GetGroundPosition().x - (character.direction.x * offset),
                                       character.GetGroundPosition().y - (character.direction.y * offset));

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
