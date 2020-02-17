using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public enum EnableMode
{
    name,
    race,
    nameAndRace
}

[CreateAssetMenu(menuName = "CharacterCreation/Property")]
public class CharacterCreatorPartProperty : ScriptableObject
{
    [BoxGroup("Enable Info")]
    [SerializeField]
    private EnableMode enableMode;

    [HideIf("enableMode", EnableMode.name)]
    [BoxGroup("Enable Info")]
    [SerializeField]
    private List<Race> restrictedRaces = new List<Race>();

    [BoxGroup("Color Info")]
    [SerializeField]
    private ColorGroup colorGroup;

    [BoxGroup("Color Info")]
    [SerializeField]
    private bool isDyeable = true;

    [BoxGroup("Part Info")]
    [SerializeField]
    private string category = "Head";

    [BoxGroup("Part Info")]
    public string parentName = "Ears";

    [BoxGroup("Part Info")]
    public string partName = "Elf Ears";



    public bool enableIt(Race race, CharacterPartData data)
    {
        if ((this.enableMode == EnableMode.race && raceEnabled(race))
         || (this.enableMode == EnableMode.nameAndRace && raceEnabled(race) && data != null)
         || (this.enableMode == EnableMode.name && data != null)) return true;

        return false;
    }

    public bool raceEnabled(Race race)
    {
        if (this.restrictedRaces.Count == 0 || this.restrictedRaces.Contains(race)) return true;
        return false;
    }
}
