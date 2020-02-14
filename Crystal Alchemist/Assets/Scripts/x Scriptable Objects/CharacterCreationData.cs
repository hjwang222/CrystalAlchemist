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

[CreateAssetMenu(menuName = "Game/Character Creation Data")]
public class CharacterCreationData : ScriptableObject, ISerializationCallbackReceiver
{
    [SerializeField]
    private SimpleSignal signal;

    public Race race;
    public string characterName;
    public bool addEarsHorns;

    public List<CharacterPartData> datas = new List<CharacterPartData>();

    [Button]
    private void UpdateCharacter()
    {
        this.signal.Raise();
    }

    public void OnAfterDeserialize() { }

    public void OnBeforeSerialize() { }
}


