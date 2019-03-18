using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu]
public class ButtonConfig : ScriptableObject, ISerializationCallbackReceiver
{

    #region Attributes

    [Header("Skill A-Button")]
    [Tooltip("Name")]
    public Sprite buttonAIcon;
    public Skill buttonA;

    [Header("Skill B-Button")]
    [Tooltip("Name")]
    public Sprite buttonBIcon;
    public Skill buttonB;

    [Header("Skill X-Button")]
    [Tooltip("Name")]
    public Sprite buttonXIcon;
    public Skill buttonX;

    [Header("Skill Y-Button")]
    [Tooltip("Name")]
    public Sprite buttonYIcon;
    public Skill buttonY;
    #endregion

    #region Functions  
    public void OnAfterDeserialize()
    {

    }

    public void OnBeforeSerialize()
    {

    }

    public GameObject getGameObjectByButton(string inputName)
    {
        switch (inputName)
        {
            case "A-Button": return buttonA.gameObject;
            case "B-Button": return buttonB.gameObject;
            case "X-Button": return buttonX.gameObject;
            case "Y-Button": return buttonY.gameObject;
            default: return null;
        }
    }

    public Skill getSkillByButton(string inputName)
    {
        switch (inputName)
        {
            case "A-Button": return buttonA;
            case "B-Button": return buttonB;
            case "X-Button": return buttonX;
            case "Y-Button": return buttonY;
            default: return null;
        }
    }

    public string getSkillNameByButton(string inputName)
    {
        switch (inputName)
        {
            case "A-Button": return buttonA.skillName;
            case "B-Button": return buttonB.skillName;
            case "X-Button": return buttonX.skillName;
            case "Y-Button": return buttonY.skillName;
            default: return null;
        }
    }


    #endregion

}


