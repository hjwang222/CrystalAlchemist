public class SkillBlendTreeModule : SkillModifier
{
    public void Initialize()
    {
        AnimatorUtil.SetAnimDirection(this.skill.direction, this.skill.animator);
    }
}
