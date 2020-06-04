using UnityEngine;

public class SkillReflector : SkillHitTrigger
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<SkillCollider2DHelper>() != null)
        {
            Skill skill = collision.GetComponent<SkillCollider2DHelper>().skill;

            if (skill != null && isReflected(skill))
            {
                skill.sender = this.skill.sender;

                if (skill.myRigidbody != null)
                {
                    skill.direction = Vector2.Reflect(skill.direction, this.skill.direction);
                    skill.GetComponent<SkillProjectile>().setVelocity();
                    skill.transform.rotation = RotationUtil.getRotation(skill.direction);
                }
            }
        }
    }

    private bool isReflected(Skill skill)
    {
        if (skill.GetComponent<SkillProjectileHit>() != null
            && skill.GetComponent<SkillProjectileHit>().canBeReflected) return true;

        return false;
    }

}
