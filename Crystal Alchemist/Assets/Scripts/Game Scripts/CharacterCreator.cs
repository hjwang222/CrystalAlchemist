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

    [ButtonGroup("Test")]
    public void setHuman()
    {
        setRace(Races.human, new Color(0.92f,0.75f, 0.6f), new Color(0.9f, 0.7f, 0.5f), new Color(0.2f, 0.6f, 0.9f));
    }


    [ButtonGroup("Test")]
    public void setElf()
    {
        setRace(Races.elves, new Color(0.95f, 0.85f, 0.75f), new Color(0.6f, 0.3f, 0.1f), new Color(0.9f, 0.3f, 0.3f));
    }

    [ButtonGroup("Test")]
    public void setLamia()
    {
        setRace(Races.lamia, new Color(0.92f, 0.75f, 0.6f), new Color(1, 0.9f, 0.9f), new Color(0.5f, 0.3f, 0f));
    }

    [ButtonGroup("Test")]
    public void setPony()
    {
        setRace(Races.ponies, new Color(0.75f, 0.75f, 0.9f), new Color(1, 0.5f, 0.8f), new Color(0.2f, 0.6f, 0.65f));
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
