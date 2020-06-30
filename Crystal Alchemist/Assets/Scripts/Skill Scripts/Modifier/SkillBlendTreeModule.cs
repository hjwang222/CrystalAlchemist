public class SkillBlendTreeModule : SkillModifier
{
    public void Initialize()
    {
        AnimatorUtil.SetAnimDirection(this.skill.GetDirection(), this.skill.animator);
    }
}
