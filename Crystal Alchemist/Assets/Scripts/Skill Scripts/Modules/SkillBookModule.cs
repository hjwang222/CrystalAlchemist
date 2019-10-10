using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SkillBookModule : MonoBehaviour
{
    [SerializeField]
    [Required]
    private StandardSkill skill;

    [BoxGroup("Pflichtfelder")]
    [Tooltip("Beschreibung des Skills")]
    [TextArea]
    public string skillDescription;

    [BoxGroup("Pflichtfelder")]
    [Tooltip("Beschreibung des Skills")]
    [TextArea]
    public string skillDescriptionEnglish;

    [BoxGroup("Pflichtfelder")]
    [Tooltip("Beschreibung des Skills")]
    [EnumToggleButtons]
    public SkillType category = SkillType.magical;

    [BoxGroup("Pflichtfelder")]
    [Tooltip("Sortierung")]
    public int orderIndex = 10;

    [FoldoutGroup("Sound und Icons", expanded: false)]
    [Tooltip("Icon für den Spieler")]
    public Sprite icon;

}
