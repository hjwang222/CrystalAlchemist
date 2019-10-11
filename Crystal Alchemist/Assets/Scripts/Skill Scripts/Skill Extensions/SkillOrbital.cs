using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SkillOrbital : MonoBehaviour
{
    [SerializeField]
    [Required]
    private StandardSkill skill;

    private void Start()
    {
        if (this.skill.target != null) this.transform.position = this.skill.target.transform.position;
    }

    private void Update()
    {
        if (this.skill.target != null) this.transform.position = this.skill.target.transform.position;
    }
    
}
