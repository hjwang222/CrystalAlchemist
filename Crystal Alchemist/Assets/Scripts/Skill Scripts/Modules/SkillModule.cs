using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SkillModule : MonoBehaviour
{
    [HideInInspector]
    public Skill skill;

    private void Awake()
    {
        this.skill = this.GetComponent<Skill>();
    }
}
