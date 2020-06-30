using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCreatorSubMenuChild : MonoBehaviour
{
    [SerializeField]
    private List<Race> restrictedRaces = new List<Race>();

    [SerializeField]
    private bool enableColor = true;

    public bool isEnabledByRace(Race race)
    {
        if (this.restrictedRaces.Count == 0 || this.restrictedRaces.Contains(race)) return true;
        return false;
    }

    public bool isEnabledByGear()
    {
        return this.enableColor;
    }
}
