using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public enum Race
{
    humiel,
    felyn,
    drea
}

public enum ColorGroup
{
    hairstyle,
    eyes,
    skin,
    scales,
    faceGear,
    headGear,
    underwear,
    upperGear,
    lowerGear,
    none
}

[System.Serializable]
public class ColorGroupData
{
    public ColorGroup colorGroup;
    public Color color = Color.white;

    public ColorGroupData(ColorGroup colorGroup, Color color)
    {
        this.colorGroup = colorGroup;
        this.color = color;
    }
}

[System.Serializable]
public class CharacterPartData
{
    public string parentName;
    public string name;

    public CharacterPartData(string parent, string name)
    {
        this.parentName = parent;
        this.name = name;
    }
}

[CreateAssetMenu(menuName = "Game/Character Preset")]
public class CharacterPreset : ScriptableObject, ISerializationCallbackReceiver
{
    [SerializeField]
    private CharacterPresetSignal signal;

    public Race race;
    public string characterName;

    public List<ColorGroupData> colorGroups = new List<ColorGroupData>();
    public List<CharacterPartData> characterParts = new List<CharacterPartData>();

    public CharacterPartData GetCharacterPartData(string parentName, string name)
    {
        foreach (CharacterPartData data in this.characterParts)
        {
            if (((data.parentName.ToUpper() == parentName.ToUpper())
                    && (name == null || data.name.ToUpper() == name.ToUpper()))) return data;
        }
        return null;
    }

    public CharacterPartData GetCharacterPartData(string parentName)
    {
        foreach (CharacterPartData characterPartData in this.characterParts)
        {
            if (characterPartData.parentName.ToUpper() == parentName.ToUpper()) return characterPartData;
        }
        return null;
    }

    public void AddCharacterPartData(string parentName, string name)
    {
        CharacterPartData characterPartData = this.GetCharacterPartData(parentName);
        if (characterPartData != null) characterPartData.name = name;
        else this.characterParts.Add(new CharacterPartData(parentName, name));
    }

    public void RemoveCharacterPartData(string parentName, string name)
    {
        CharacterPartData characterPartData = this.GetCharacterPartData(parentName, name);
        if (characterPartData != null) this.characterParts.Remove(characterPartData);
    }

    public void RemoveCharacterPartData(string parentName)
    {
        CharacterPartData characterPartData = this.GetCharacterPartData(parentName);
        if (characterPartData != null) this.characterParts.Remove(characterPartData);
    }

    public ColorGroupData GetColorGroupData(ColorGroup colorGroup)
    {
        foreach (ColorGroupData colorGroupData in this.colorGroups)
        {
            if (colorGroupData.colorGroup == colorGroup) return colorGroupData;
        }
        return null;
    }

    public void AddColorGroup(ColorGroup colorGroup, Color color)
    {
        ColorGroupData colorGroupData = this.GetColorGroupData(colorGroup);
        if (colorGroupData != null) colorGroupData.color = color;
        else this.colorGroups.Add(new ColorGroupData(colorGroup, color));
    }

    public void RemoveColorGroup(ColorGroup colorGroup)
    {
        ColorGroupData colorGroupData = this.GetColorGroupData(colorGroup);
        if (colorGroupData != null) this.colorGroups.Remove(colorGroupData);
    }

    public Color getColor(ColorGroup colorGroup)
    {
        foreach (ColorGroupData data in this.colorGroups)
        {
            if (data.colorGroup == colorGroup) return data.color;
        }
        return Color.white;
    }

    [Button]
    public void UpdateCharacter()
    {
        this.signal.Raise(this);
    }

    public void OnAfterDeserialize() { }

    public void OnBeforeSerialize() { }
}


