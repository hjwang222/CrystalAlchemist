using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Skill))]
public class SkillModule : MonoBehaviour
{
    [HideInInspector]
    public Skill skill;

    private void Awake()
    {
        this.skill = this.GetComponent<Skill>();
    }
}
