using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

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


    [Button]
    public void setColor()
    {
        foreach(GameObject temp in this.parts)
        {
            temp.SetActive(false);
        }

        foreach(Race temp in this.races)
        {
            if(temp.race == this.race)
            {
                foreach (GameObject temp1 in temp.parts)
                {
                    temp1.SetActive(true);
                }
            }
        }

        foreach(SpriteRenderer temp in this.bodyParts)
        {
            temp.color = this.bodyColor;
        }

        foreach (SpriteRenderer temp in this.hairParts)
        {
            temp.color = this.hairColor;
        }

        this.eyePart.color = this.eyeColor;
    }

}
