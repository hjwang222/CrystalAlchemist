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
}

[System.Serializable]
public class CharacterPartData
{
    public string name;
    public string parentName;
    public List<Sprite> previewImages = new List<Sprite>();

    public void setSprites(Sprite front, Sprite back)
    {
        this.previewImages.Clear();
        this.previewImages.Add(front);
        this.previewImages.Add(back);
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

    public CharacterPartData getData(string name, string parentName)
    {
        foreach (CharacterPartData data in this.characterParts)
        {
            if (((data.parentName.ToUpper() == parentName.ToUpper())
                    && (name == null || data.name.ToUpper() == name.ToUpper()))) return data;
        }
        return null;
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
    private void UpdateCharacter()
    {
        this.signal.Raise(this);
    }

    public void OnAfterDeserialize() { }

    public void OnBeforeSerialize() { }
}


