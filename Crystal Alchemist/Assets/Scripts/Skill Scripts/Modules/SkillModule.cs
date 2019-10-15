using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SkillModule : MonoBehaviour
{

    public Skill skill;

    private void Awake()
    {
        this.skill = this.GetComponent<Skill>();
    }
}
