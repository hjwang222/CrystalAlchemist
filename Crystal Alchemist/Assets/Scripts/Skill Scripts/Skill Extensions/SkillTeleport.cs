using UnityEngine;
using Sirenix.OdinInspector;

public class SkillTeleport : SkillExtension
{
    public override void Initialize() => GameEvents.current.DoReturn();
}