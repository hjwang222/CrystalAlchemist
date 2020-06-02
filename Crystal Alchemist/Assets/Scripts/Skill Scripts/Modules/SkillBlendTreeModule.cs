public class SkillBlendTreeModule : SkillModule
{
    public void Initialize()
    {
        AnimatorUtil.SetAnimDirection(this.skill.direction, this.skill.animator);
    }
}
