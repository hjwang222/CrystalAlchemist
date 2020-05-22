using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public enum EnableMode
{
    name,
    race,
    nameAndRace
}

[CreateAssetMenu(menuName = "Game/CharacterCreation/Property")]
public class CharacterCreatorPartProperty : ScriptableObject
{
    [BoxGroup("Enable Info")]
    public bool neverDisable = false;

    [HideIf("neverDisable", true)]
    [BoxGroup("Enable Info")]
    [SerializeField]
    private EnableMode enableMode;

    [HideIf("neverDisable", true)]
    [HideIf("enableMode", EnableMode.name)]
    [BoxGroup("Enable Info")]
    [SerializeField]
    private List<Race> restrictedRaces = new List<Race>();

    [BoxGroup("Color Info")]
    public List<ColorTablePlayer> colorTables = new List<ColorTablePlayer>();

    [BoxGroup("Part Info")]
    public string category = "Head";

    [BoxGroup("Part Info")]
    public string parentName = "Ears";

    [BoxGroup("Part Info")]
    public string partName = "Elf Ears";


    public string getFullPath()
    {
        return this.category + "/" + this.parentName + "/" + this.partName + ".png";
    }

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
