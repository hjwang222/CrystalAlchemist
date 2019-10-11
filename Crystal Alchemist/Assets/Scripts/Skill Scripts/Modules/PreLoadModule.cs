using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class PreLoadModule : MonoBehaviour
{
    [Required]
    public Skill skill;

    public virtual void checkRequirements()
    {

    }
}
