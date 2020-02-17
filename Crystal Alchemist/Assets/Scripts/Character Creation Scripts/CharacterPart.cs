using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;

public enum EnableMode
{
    always,
    race,
    nameAndRace
}

public class CharacterPart : MonoBehaviour
{
    public List<Race> restrictedRaces = new List<Race>();
    public ColorGroup colorGroup;

    public EnableMode enableMode;
    public bool dyeable = true;

    public string assetPath;

    public bool raceEnabled(Race race)
    {
        if (this.restrictedRaces.Count == 0 || this.restrictedRaces.Contains(race)) return true;
        return false;
    }

    public bool enableIt(Race race, CharacterPartData data)
    {
        if ((this.enableMode == EnableMode.always)
            || (this.enableMode == EnableMode.race && raceEnabled(race))
            || (this.enableMode == EnableMode.nameAndRace && raceEnabled(race) && data != null)) return true;

        return false;
    }
}
