using UnityEngine;
using Sirenix.OdinInspector;
using AssetIcons;

[CreateAssetMenu(menuName = "Game/Ability/Skill Book Info")]
public class SkillBookInfo : ScriptableObject
{
    [BoxGroup("Pflichtfelder")]
    [Tooltip("Beschreibung des Skills")]
    [TextArea]
    [SerializeField]
    private string skillDescription;

    [BoxGroup("Pflichtfelder")]
    [Tooltip("Beschreibung des Skills")]
    [TextArea]
    [SerializeField]
    private string skillDescriptionEnglish;

    [BoxGroup("Pflichtfelder")]
    [Tooltip("Beschreibung des Skills")]
    [EnumToggleButtons]
    public SkillType category = SkillType.magical;

    [BoxGroup("Pflichtfelder")]
    [Tooltip("Sortierung")]
    public int orderIndex = 10;

    [BoxGroup("Sound und Icons")]
    [Tooltip("Icon für den Spieler")]
    [AssetIcon]
    public Sprite icon;

    public string getDescription()
    {
        return FormatUtil.getLanguageDialogText(this.skillDescription, this.skillDescriptionEnglish);
    }
}
