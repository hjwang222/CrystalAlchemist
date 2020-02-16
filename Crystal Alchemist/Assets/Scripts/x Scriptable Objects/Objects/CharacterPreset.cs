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
    lowerGear
}

[System.Serializable]
public class CharacterPartData
{
    public ColorGroup type;
    public Color color = Color.white;
    public string name;
}

[System.Serializable]
public class CharacterPreviewData
{
    public string previewPart;
    public Sprite[] previewImages = new Sprite[2];
    public Color previewColor;

    public CharacterPreviewData(string name, Sprite front, Sprite back, Color color)
    {
        this.previewPart = name;
        this.previewImages[0] = front;
        this.previewImages[1] = back;
        this.previewColor = color;
    }
}

    [CreateAssetMenu(menuName = "Game/Character Preset")]
public class CharacterPreset : ScriptableObject, ISerializationCallbackReceiver
{
    [SerializeField]
    private CharacterPresetSignal signal;

    public Race race;
    public string characterName;
    public bool addEarsHorns;
    public List<CharacterPreviewData> preview = new List<CharacterPreviewData>();

    public List<CharacterPartData> datas = new List<CharacterPartData>();

    public void setPreview(string name, Sprite previewFront, Sprite previewBack, Color color)
    {
        CharacterPreviewData temp = getPreviewData(name);

        if(temp == null)
        {
            this.preview.Add(new CharacterPreviewData(name, previewFront, previewBack, color));
        }
        else
        {
            temp.previewImages[0] = previewFront;
            temp.previewImages[1] = previewBack;
            temp.previewColor = color;
        }     
    }

    public CharacterPreviewData getPreviewData(string name)
    {
        foreach (CharacterPreviewData previewData in this.preview)
        {
            if (previewData.previewPart.ToUpper() == name.ToUpper()) return previewData;
        }
        return null;
    }

    [Button]
    private void UpdateCharacter()
    {
        this.signal.Raise(this);
    }

    public void OnAfterDeserialize() { }

    public void OnBeforeSerialize() { }
}


