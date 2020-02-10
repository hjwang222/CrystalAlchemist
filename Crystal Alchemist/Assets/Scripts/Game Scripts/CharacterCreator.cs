using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEditor;
using System.IO;
using System.Linq;

public enum Races
{
    human,
    elves,
    lamia,
    ponies
}

[System.Serializable]
public struct Race
{
    public Races race;
    public List<GameObject> parts;
}

[System.Serializable]
public struct Preset
{
    public Races race;
    public Color hair;
    public Color skin;
    public Color eyes;
}

public class CharacterCreator : MonoBehaviour
{
    [FoldoutGroup("Internal", Expanded = false)]
    [SerializeField]
    private List<SpriteRenderer> bodyParts = new List<SpriteRenderer>();

    [FoldoutGroup("Internal", Expanded = false)]
    [SerializeField]
    private List<SpriteRenderer> hairParts = new List<SpriteRenderer>();

    [FoldoutGroup("Internal", Expanded = false)]
    [SerializeField]
    private SpriteRenderer eyePart = new SpriteRenderer();

    [FoldoutGroup("Internal", Expanded = false)]
    [SerializeField]
    private List<Race> races = new List<Race>();

    [FoldoutGroup("Internal", Expanded = false)]
    [SerializeField]
    private List<GameObject> parts = new List<GameObject>();

    [FoldoutGroup("Internal", Expanded = false)]
    [SerializeField]
    private List<Preset> presets = new List<Preset>();

    [BoxGroup]
    [SerializeField]
    private Color hairColor;

    [BoxGroup]
    [SerializeField]
    private Color eyeColor;

    [BoxGroup]
    [SerializeField]
    private Color bodyColor;

    [BoxGroup]
    [SerializeField]
    private Races race;

    [ButtonGroup("Test")]
    public void setRace()
    {
        setRace(this.race, this.bodyColor, this.hairColor, this.eyeColor);
    }

    private void setPreset(Races race)
    {
        foreach(Preset preset in this.presets)
        {
            if(preset.race == race)
            {
                setRace(preset.race, preset.skin, preset.hair, preset.eyes);
                break;
            }
        }
    }

    [ButtonGroup("Test")]
    public void setHuman()
    {
        setPreset(Races.human);
    }


    [ButtonGroup("Test")]
    public void setElf()
    {
        setPreset(Races.elves);
    }

    [ButtonGroup("Test")]
    public void setLamia()
    {
        setPreset(Races.lamia);
    }

    [ButtonGroup("Test")]
    public void setPony()
    {
        setPreset(Races.ponies);
    }
         
    private void setRace(Races ra, Color body, Color hair, Color eyes)
    {
        foreach (GameObject temp in this.parts)
        {
            temp.SetActive(false);
        }

        foreach (Race temp in this.races)
        {
            if (temp.race == ra)
            {
                foreach (GameObject temp1 in temp.parts)
                {
                    temp1.SetActive(true);
                }
            }
        }

        foreach (SpriteRenderer temp in this.bodyParts)
        {
            temp.color = body;
        }

        foreach (SpriteRenderer temp in this.hairParts)
        {
            temp.color = hair;
        }

        this.eyePart.color = eyes;
    }
        
}
