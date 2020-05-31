using UnityEngine;

public class SkillHitTrigger : MonoBehaviour
{
    [HideInInspector]
    public Skill skill;

    private void Awake() => this.skill = this.GetComponent<Skill>();
}

