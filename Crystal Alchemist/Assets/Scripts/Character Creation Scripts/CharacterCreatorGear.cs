using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnableMode
{
    race,
    nameAndRace
}

public class CharacterCreatorGear : CharacterCreatorButton
{
    [SerializeField]
    private List<Race> restrictedRaces = new List<Race>();
    [SerializeField]
    private ColorGroup colorGroup;

    [SerializeField]
    private EnableMode enableMode;

    [SerializeField]
    private string category = "Head";
    public string parentName = "Ears";
    public string partName = "Elf Ears";

    [SerializeField]
    private bool isDyeable = true;

    [SerializeField]
    private SimpleSignal colorGroupSignal;

    public override void Click()
    {
        test();
        base.Click();
    }



    public bool enableIt(Race race, CharacterPartData data)
    {
        if ((this.enableMode == EnableMode.race && raceEnabled(race))
         || (this.enableMode == EnableMode.nameAndRace && raceEnabled(race) && data != null)) return true;

        return false;
    }

    public bool raceEnabled(Race race)
    {
        if (this.restrictedRaces.Count == 0 || this.restrictedRaces.Contains(race)) return true;
        return false;
    }

    public void test()
    {
        if (this.restrictedRaces.Count > 0
        && !this.restrictedRaces.Contains(this.creatorPreset.race))
        {
            this.creatorPreset.RemoveCharacterPartData(this.parentName, this.partName);
        }
        else
        {
            this.creatorPreset.AddCharacterPartData(this.parentName, this.partName);
            ColorGroupData colorGroupData = this.creatorPreset.GetColorGroupData(this.colorGroup);
            if (!isDyeable) this.creatorPreset.colorGroups.Remove(colorGroupData);
            else this.creatorPreset.AddColorGroup(this.colorGroup, Color.white); //need to be stored
        }
    }




}
